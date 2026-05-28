using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MuseumExhibits.API;
using MuseumExhibits.API.Endpoints;
using MuseumExhibits.Application.Mapping;
using MuseumExhibits.Application.Services;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Infrastructure.Cloud;
using MuseumExhibits.Infrastructure.Data;
using MuseumExhibits.Infrastructure.Repositories;
using System.Text;
using System.Threading.RateLimiting;
using MuseumExhibits.Application.Abstractions;

var builder = WebApplication.CreateBuilder(args);
var config  = builder.Configuration;

var jwtSecret = config["JwtSettings:SecretKey"]
    ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");

var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<MuseumExhibitsDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("MsSqlConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IExhibitRepository, ExhibitRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<ICloudImageClient, CloudImageClient>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IExhibitService, ExhibitService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICollectionService, CollectionService>();

builder.Services.AddSingleton(new Cloudinary(new Account(
    config["Cloudinary:CloudName"],
    config["Cloudinary:ApiKey"],
    config["Cloudinary:ApiSecret"]
)));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("GlobalLimiter", o =>
    {
        o.PermitLimit = 100;
        o.Window = TimeSpan.FromMinutes(1);
    });

    options.AddTokenBucketLimiter("LoginLimiter", o =>
    {
        o.TokenLimit = 10;
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        o.ReplenishmentPeriod = TimeSpan.FromMinutes(5);
        o.TokensPerPeriod = 1;
        o.QueueLimit = 0;
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseCors();
app.UseStaticFiles();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

var api = app.MapGroup("/api");

api.MapGroup("/auth")
   .RequireRateLimiting("LoginLimiter")
   .MapAuthEndpoints();

var global = api.MapGroup(string.Empty)
               .RequireRateLimiting("GlobalLimiter");

global.MapGroup("/exhibits").MapExhibitEndpoints();
global.MapGroup("/categories").MapCategoryEndpoints();
global.MapGroup("/images").MapImageEndpoints();
global.MapGroup("/posts").MapPostEndpoints();
global.MapGroup("/collections").MapCollectionEndpoints();

app.Run();

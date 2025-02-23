using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.Mapping;
using MuseumExhibits.Application.Services;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Infrastructure.Cloud;
using MuseumExhibits.Infrastructure.Repostories;
using MuseumExhibits.Infrastructure.Data;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });

});


builder.Services.AddDbContext<MuseumExhibitsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IExhibitRepository, ExhibitRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

builder.Services.AddScoped<ICloudImageClient, CloudImageClient>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IExhibitService, ExhibitService>();
builder.Services.AddScoped<IImageService, ImageService>();


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


builder.Services.AddSingleton(new Cloudinary(new Account("cloud", "apiKey", "apiSecret")));

var app = builder.Build();



app.UseHsts();


app.UseSwagger();
app.UseSwaggerUI();


app.UseStaticFiles();

app.UseRouting();
app.UseCors();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



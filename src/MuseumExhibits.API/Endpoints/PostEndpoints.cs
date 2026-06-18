using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using System.Security.Claims;

namespace MuseumExhibits.API.Endpoints;

public static class PostEndpoints
{
    private static readonly string[] AllowedMimeTypes =
        ["image/jpeg", "image/png", "image/webp", "image/gif"];

    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    public static RouteGroupBuilder MapPostEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Posts");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create).RequireAuthorization();
        group.MapPost("/upload-image", UploadImage).RequireAuthorization().DisableAntiforgery();
        group.MapPut("/{id:guid}", Update).RequireAuthorization();
        group.MapDelete("/{id:guid}", Delete).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> UploadImage(
        [FromForm] PostImageRequest dto,
        ICloudImageClient cloudClient)
    {
        if (dto.File is null)
            return Results.BadRequest(new { message = "No file provided." });

        if (dto.File.Length > MaxFileSizeBytes)
            return Results.BadRequest(new { message = "File exceeds the 10 MB size limit." });

        if (!AllowedMimeTypes.Contains(dto.File.ContentType.ToLowerInvariant()))
            return Results.BadRequest(new { message = "Only JPEG, PNG, WebP, and GIF images are allowed." });

        var result = await cloudClient.UploadImageAsync(dto.File, "post_images");
        return Results.Ok(new { url = result.Url });
    }

    private static async Task<IResult> GetAll(
        [AsParameters] PostQueryParameters query,
        IPostService service,
        ClaimsPrincipal user)
    {
        bool isAdmin = user.Identity?.IsAuthenticated == true;
        var result   = await service.Get(query, isAdmin);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetById(Guid id, IPostService service)
    {
        var post = await service.GetById(id);
        return Results.Ok(post);
    }

    private static async Task<IResult> Create(PostRequest request, IPostService service)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        var id = await service.Create(request);
        return Results.Created($"/api/posts/{id}", id);
    }

    private static async Task<IResult> Update(Guid id, PostRequest request, IPostService service)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        await service.Update(id, request);
        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, IPostService service)
    {
        await service.Delete(id);
        return Results.NoContent();
    }
}

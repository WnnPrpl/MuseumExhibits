using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.API.Endpoints;

public static class ImageEndpoints
{
    public static RouteGroupBuilder MapImageEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Images");

        group.MapGet("/{entityId:guid}", GetByEntityId);
        group.MapPost("/{entityId:guid}", Upload).RequireAuthorization().DisableAntiforgery();
        group.MapDelete("/{imageId:guid}", Delete).RequireAuthorization();
        group.MapDelete("/entity/{entityId:guid}", DeleteByEntity).RequireAuthorization();
        group.MapPut("/{entityId:guid}/title/{imageId:guid}", SetTitle).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetByEntityId(Guid entityId, IImageService service)
    {
        var images = await service.GetByEntityId(entityId);
        return Results.Ok(images);
    }

    private static async Task<IResult> Upload(
        Guid entityId,
        [FromForm] ImageRequest fileDTO,
        IImageService service)
    {
        var response = await service.UploadImage(entityId, fileDTO);
        return Results.Created($"/api/images/{entityId}", response);
    }

    private static async Task<IResult> Delete(Guid imageId, IImageService service)
    {
        await service.DeleteImage(imageId);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteByEntity(Guid entityId, IImageService service)
    {
        await service.DeleteByEntityId(entityId);
        return Results.NoContent();
    }

    private static async Task<IResult> SetTitle(Guid entityId, Guid imageId, IImageService service)
    {
        await service.SetTitleImage(entityId, imageId);
        return Results.NoContent();
    }
}

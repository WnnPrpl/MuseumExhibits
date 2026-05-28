using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.API.Endpoints;

public static class CollectionEndpoints
{
    public static RouteGroupBuilder MapCollectionEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Collections");

        group.MapGet("/", GetAll);
        group.MapGet("/paged", GetPaged);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create).RequireAuthorization();
        group.MapPut("/{id:guid}", Update).RequireAuthorization();
        group.MapDelete("/{id:guid}", Delete).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetAll(ICollectionService service)
    {
        var collections = await service.GetAll();
        return Results.Ok(collections);
    }

    private static async Task<IResult> GetPaged(
        ICollectionService service,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1)
            return Results.BadRequest("Page and pageSize must be greater than zero.");

        var result = await service.GetPaged(page, pageSize);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetById(Guid id, ICollectionService service)
    {
        var collection = await service.GetById(id);
        return Results.Ok(collection);
    }

    private static async Task<IResult> Create(CollectionRequest request, ICollectionService service)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        var id = await service.Create(request);
        return Results.Created($"/api/collections/{id}", id);
    }

    private static async Task<IResult> Update(Guid id, CollectionRequest request, ICollectionService service)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        await service.Update(id, request);
        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, ICollectionService service)
    {
        await service.Delete(id);
        return Results.NoContent();
    }
}

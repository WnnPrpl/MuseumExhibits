using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.API.Endpoints;

public static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoryEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Categories");

        group.MapGet("/", GetAll);
        group.MapGet("/paged", GetPaged);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create).RequireAuthorization();
        group.MapPut("/{id:guid}", Update).RequireAuthorization();
        group.MapDelete("/{id:guid}", Delete).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetAll(ICategoryService service)
    {
        var categories = await service.GetAll();
        return Results.Ok(categories);
    }

    private static async Task<IResult> GetPaged(
        ICategoryService service,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1)
            return Results.BadRequest("Page and pageSize must be greater than zero.");

        var result = await service.GetByPage(page, pageSize);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetById(Guid id, ICategoryService service)
    {
        var category = await service.GetById(id);
        return Results.Ok(category);
    }

    private static async Task<IResult> Create(CategoryDTO dto, ICategoryService service)
    {
        dto.Id = Guid.NewGuid();

        if (!ValidationHelper.TryValidate(dto, out var errors))
            return Results.ValidationProblem(errors);

        var id = await service.Create(dto);
        return Results.Created($"/api/categories/{id}", id);
    }

    private static async Task<IResult> Update(Guid id, CategoryDTO dto, ICategoryService service)
    {
        dto.Id = id;

        if (!ValidationHelper.TryValidate(dto, out var errors))
            return Results.ValidationProblem(errors);

        await service.Update(id, dto);
        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, ICategoryService service)
    {
        await service.Delete(id);
        return Results.NoContent();
    }
}

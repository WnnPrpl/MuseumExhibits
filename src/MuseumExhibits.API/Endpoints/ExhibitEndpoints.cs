using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using System.Security.Claims;

namespace MuseumExhibits.API.Endpoints;

public static class ExhibitEndpoints
{
    public static RouteGroupBuilder MapExhibitEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Exhibits");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create).RequireAuthorization();
        group.MapPut("/{id:guid}", Update).RequireAuthorization();
        group.MapDelete("/{id:guid}", Delete).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetAll(
        [AsParameters] ExhibitQueryParameters query,
        IExhibitService service,
        ClaimsPrincipal user)
    {
        bool isAdmin = user.Identity?.IsAuthenticated == true;
        var result   = await service.Get(query, isAdmin);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetById(Guid id, IExhibitService service)
    {
        var exhibit = await service.GetById(id);
        return Results.Ok(exhibit);
    }

    private static async Task<IResult> Create(ExhibitRequest request, IExhibitService service)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        var id = await service.Create(request);
        return Results.Created($"/api/exhibits/{id}", id);
    }

    private static async Task<IResult> Update(Guid id, ExhibitRequest request, IExhibitService service)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        await service.Update(id, request);
        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, IExhibitService service)
    {
        await service.Delete(id);
        return Results.NoContent();
    }
}

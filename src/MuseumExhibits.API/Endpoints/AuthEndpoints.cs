using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.API.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Auth");

        group.MapPost("/register", Register).WithName("Auth_Register");
        group.MapPost("/login", Login).WithName("Auth_Login");

        return group;
    }

    private static async Task<IResult> Register(
        RegisterRequest request,
        IAuthService authService)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        try
        {
            var token = await authService.RegisterAsync(request);
            return Results.Ok(new { Token = token });
        }
        catch (Exception ex) when (ex.Message.Contains("already exists"))
        {
            return Results.Conflict(new { Error = ex.Message });
        }
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        IAuthService authService)
    {
        if (!ValidationHelper.TryValidate(request, out var errors))
            return Results.ValidationProblem(errors);

        var token = await authService.LoginAsync(request);
        return Results.Ok(new { Token = token });
    }
}

using System.Net;
using System.Net.Http.Json;

namespace MuseumExhibits.Tests;

[Collection("Api")]
public class AuthTests(ApiFixture api)
{
    [Fact]
    public async Task Login_ValidCredentials_Returns200()
    {
        var response = await api.Client.PostAsJsonAsync("auth/login", new
        {
            email    = "admin@example.com",
            password = "AdminTestPassword5202!"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_WrongPassword_Returns400()
    {
        var response = await api.Client.PostAsJsonAsync("auth/login", new
        {
            email    = "admin@example.com",
            password = "wrongpassword"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_EmptyFields_Returns400()
    {
        var response = await api.Client.PostAsJsonAsync("auth/login", new
        {
            email    = "",
            password = ""
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

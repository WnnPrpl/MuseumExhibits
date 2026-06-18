using System.Net;
using System.Net.Http.Json;

namespace MuseumExhibits.Tests;

[Collection("Api")]
public class PostTests(ApiFixture api)
{
    [Fact]
    public async Task GetPosts_NoAuth_Returns200()
    {
        var response = await api.Client.GetAsync("posts");
        var body     = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, $"{response.StatusCode}: {body}");
    }

    [Fact]
    public async Task CreatePost_ValidData_Returns201()
    {
        var response = await api.Client.PostAsJsonAsync("posts", new
        {
            title        = $"Тест {Guid.NewGuid():N}",
            content      = "Повний текст публікації",
            shortContent = "Короткий опис",
            visible      = true
        });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task DeletePost_Returns204()
    {
        var create = await api.Client.PostAsJsonAsync("posts", new
        {
            title   = $"Тест {Guid.NewGuid():N}",
            content = "Текст"
        });
        var id     = (await create.Content.ReadAsStringAsync()).Trim('"');

        var delete = await api.Client.DeleteAsync($"posts/{id}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

    [Fact]
    public async Task CreatePost_MissingRequiredFields_Returns400()
    {
        var response = await api.Client.PostAsJsonAsync("posts", new { shortContent = "Тільки короткий опис" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

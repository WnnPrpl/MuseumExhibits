using System.Net;
using System.Net.Http.Json;

namespace MuseumExhibits.Tests;

[Collection("Api")]
public class CategoryTests(ApiFixture api)
{
    [Fact]
    public async Task GetCategories_NoAuth_Returns200()
    {
        var response = await api.Client.GetAsync("categories");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateCategory_ValidName_Returns201()
    {
        var name     = $"Тест {Guid.NewGuid():N}";
        var response = await api.Client.PostAsJsonAsync("categories", new { name });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCategory_Returns204()
    {
        var create = await api.Client.PostAsJsonAsync("categories", new { name = $"Тест {Guid.NewGuid():N}" });
        var id     = (await create.Content.ReadAsStringAsync()).Trim('"');

        var delete = await api.Client.DeleteAsync($"categories/{id}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

    [Fact]
    public async Task CreateCategory_EmptyName_Returns400()
    {
        var response = await api.Client.PostAsJsonAsync("categories", new { name = "" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

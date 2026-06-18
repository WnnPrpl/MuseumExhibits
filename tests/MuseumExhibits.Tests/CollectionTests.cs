using System.Net;
using System.Net.Http.Json;

namespace MuseumExhibits.Tests;

[Collection("Api")]
public class CollectionTests(ApiFixture api)
{
    [Fact]
    public async Task GetCollections_NoAuth_Returns200()
    {
        var response = await api.Client.GetAsync("collections");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateCollection_ValidData_Returns201()
    {
        var response = await api.Client.PostAsJsonAsync("collections", new
        {
            name        = $"Тест {Guid.NewGuid():N}",
            description = "Опис"
        });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCollection_Returns204()
    {
        var create = await api.Client.PostAsJsonAsync("collections", new { name = $"Тест {Guid.NewGuid():N}" });
        var id     = (await create.Content.ReadAsStringAsync()).Trim('"');

        var delete = await api.Client.DeleteAsync($"collections/{id}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

    [Fact]
    public async Task CreateCollection_EmptyName_Returns400()
    {
        var response = await api.Client.PostAsJsonAsync("collections", new { name = "" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

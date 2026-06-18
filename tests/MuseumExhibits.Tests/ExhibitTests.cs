using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace MuseumExhibits.Tests;

[Collection("Api")]
public class ExhibitTests(ApiFixture api)
{
    private static readonly object ValidExhibit = new
    {
        name                = "Глиняний глечик",
        authorOrManufacturer = "Невідомий майстер",
        classification      = "Кераміка",
        material            = "Глина",
        preservationState   = "Задовільний",
        registrationNumber  = "КЕР-001",
        responsibleKeeper   = "Коваленко О.М.",
        enteredBy           = "Петренко В.І.",
        fullDescription     = "Глиняний глечик XIX ст.",
        visible             = true
    };

    [Fact]
    public async Task GetExhibits_NoAuth_Returns200()
    {
        var response = await api.Client.GetAsync("exhibits");
        var body     = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, $"{response.StatusCode}: {body}");
    }

    [Fact]
    public async Task GetExhibit_NonExistentId_Returns404()
    {
        var response = await api.Client.GetAsync("exhibits/00000000-0000-0000-0000-000000000000");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateExhibit_NoToken_Returns401()
    {
        using var client = new HttpClient { BaseAddress = api.Client.BaseAddress };
        var response     = await client.PostAsJsonAsync("exhibits", ValidExhibit);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateExhibit_InvalidToken_Returns401()
    {
        using var client = new HttpClient { BaseAddress = api.Client.BaseAddress };
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "invalid.token.here");
        var response = await client.PostAsJsonAsync("exhibits", ValidExhibit);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateExhibit_MissingRequiredFields_Returns400()
    {
        var response = await api.Client.PostAsJsonAsync("exhibits", new { name = "Без обов'язкових полів" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateExhibit_NonExistentCategory_Returns404()
    {
        var response = await api.Client.PostAsJsonAsync("exhibits", new
        {
            name                = "Експонат",
            authorOrManufacturer = "Автор",
            classification      = "Клас",
            material            = "Матеріал",
            preservationState   = "Добрий",
            registrationNumber  = "003",
            responsibleKeeper   = "Іванов",
            enteredBy           = "Петров",
            fullDescription     = "Опис",
            categoryId          = "00000000-0000-0000-0000-000000000000"
        });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteExhibit_WithImages_DeletesCascade_Returns204()
    {
        var create    = await api.Client.PostAsJsonAsync("exhibits", ValidExhibit);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        var exhibitId = (await create.Content.ReadAsStringAsync()).Trim('"');

        // Завантажуємо зображення
        using var form  = new MultipartFormDataContent();
        var png         = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwADhQGAWjR9awAAAABJRU5ErkJggg==");
        var fileContent = new ByteArrayContent(png);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        form.Add(fileContent, "file", "test.png");
        form.Add(new StringContent("true"), "isTitleImage");
        var upload = await api.Client.PostAsync($"images/{exhibitId}", form);
        Assert.Equal(HttpStatusCode.Created, upload.StatusCode);

        // Видаляємо експонат — зображення мають видалитись автоматично
        var delete = await api.Client.DeleteAsync($"exhibits/{exhibitId}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        // Перевіряємо що зображення більше немає
        var images = await api.Client.GetAsync($"images/{exhibitId}");
        var body   = await images.Content.ReadAsStringAsync();
        Assert.Equal("[]", body.Trim());
    }

    [Fact]
    public async Task DeleteExhibit_NonExistentId_Returns404()
    {
        var response = await api.Client.DeleteAsync("exhibits/00000000-0000-0000-0000-000000000000");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

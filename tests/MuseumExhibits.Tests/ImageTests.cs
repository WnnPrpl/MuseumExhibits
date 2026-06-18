using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MuseumExhibits.Tests;

[Collection("Api")]
public class ImageTests(ApiFixture api)
{
    // Мінімальний валідний PNG 1x1 піксель
    private static readonly byte[] MinimalPng = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwADhQGAWjR9awAAAABJRU5ErkJggg==");

    private static readonly object ValidExhibit = new
    {
        name                 = "Тест зображень",
        authorOrManufacturer = "Автор",
        classification       = "Клас",
        material             = "Матеріал",
        preservationState    = "Добрий",
        registrationNumber   = $"IMG-{Guid.NewGuid():N}",
        responsibleKeeper    = "Іванов",
        enteredBy            = "Петров",
        fullDescription      = "Опис"
    };

    [Fact]
    public async Task UploadImage_ValidPng_Returns201()
    {
        var exhibitId = await CreateExhibitAsync();

        var upload = await UploadImageAsync(exhibitId, isTitleImage: false);
        Assert.Equal(HttpStatusCode.Created, upload.StatusCode);

        await CleanupExhibitAsync(exhibitId);
    }

    [Fact]
    public async Task UploadImage_WrongMimeType_Returns400()
    {
        var exhibitId = await CreateExhibitAsync();

        using var form      = new MultipartFormDataContent();
        var fileContent     = new ByteArrayContent(new byte[] { 0x00, 0x01, 0x02 });
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        form.Add(fileContent, "file", "test.txt");
        form.Add(new StringContent("false"), "isTitleImage");

        var response = await api.Client.PostAsync($"images/{exhibitId}", form);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await CleanupExhibitAsync(exhibitId);
    }

    [Fact]
    public async Task UploadImage_ExceedsSizeLimit_Returns400()
    {
        var exhibitId   = await CreateExhibitAsync();
        var bigFile     = new byte[11 * 1024 * 1024]; // 11 MB

        using var form  = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(bigFile);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        form.Add(fileContent, "file", "big.png");
        form.Add(new StringContent("false"), "isTitleImage");

        var response = await api.Client.PostAsync($"images/{exhibitId}", form);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await CleanupExhibitAsync(exhibitId);
    }

    [Fact]
    public async Task GetImages_ByExhibitId_Returns200()
    {
        var exhibitId = await CreateExhibitAsync();

        var response = await api.Client.GetAsync($"images/{exhibitId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await CleanupExhibitAsync(exhibitId);
    }

    [Fact]
    public async Task SetTitleImage_Returns204()
    {
        var exhibitId = await CreateExhibitAsync();
        var upload    = await UploadImageAsync(exhibitId, isTitleImage: false);
        var imageId   = await ReadIdAsync(upload);

        var setTitle = await api.Client.PutAsync($"images/{exhibitId}/title/{imageId}", null);
        Assert.Equal(HttpStatusCode.NoContent, setTitle.StatusCode);

        await CleanupExhibitAsync(exhibitId);
    }

    [Fact]
    public async Task DeleteImage_Returns204()
    {
        var exhibitId = await CreateExhibitAsync();
        var upload    = await UploadImageAsync(exhibitId, isTitleImage: false);
        var imageId   = await ReadIdAsync(upload);

        var delete = await api.Client.DeleteAsync($"images/{imageId}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        await CleanupExhibitAsync(exhibitId);
    }

    private async Task<string> CreateExhibitAsync()
    {
        var response = await api.Client.PostAsJsonAsync("exhibits", ValidExhibit);
        return (await response.Content.ReadAsStringAsync()).Trim('"');
    }

    private async Task CleanupExhibitAsync(string exhibitId) =>
        await api.Client.DeleteAsync($"exhibits/{exhibitId}");

    private async Task<HttpResponseMessage> UploadImageAsync(string exhibitId, bool isTitleImage)
    {
        using var form = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(MinimalPng);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        form.Add(fileContent, "file", "test.png");
        form.Add(new StringContent(isTitleImage.ToString().ToLower()), "isTitleImage");
        return await api.Client.PostAsync($"images/{exhibitId}", form);
    }

    private static async Task<string> ReadIdAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("id").GetString() ?? string.Empty;
    }
}

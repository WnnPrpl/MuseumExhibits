using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MuseumExhibits.Tests;

public class ApiFixture : IAsyncLifetime
{
    public HttpClient Client { get; } = new(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    })
    {
        BaseAddress = new Uri("https://localhost:7128/api/")
    };

    public string Token { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        var response = await Client.PostAsJsonAsync("auth/login", new
        {
            email    = "admin@example.com",
            password = "AdminTestPassword5202!"
        });

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Token    = json.GetProperty("token").GetString() ?? string.Empty;
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);
        }
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        return Task.CompletedTask;
    }
}

[CollectionDefinition("Api")]
public class ApiCollection : ICollectionFixture<ApiFixture> { }

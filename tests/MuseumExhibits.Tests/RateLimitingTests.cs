using System.Net;
using System.Net.Http.Json;

namespace MuseumExhibits.Tests;

[Collection("Api")]
public class RateLimitingTests(ApiFixture api)
{
    [Fact]
    public async Task Login_ExceedRateLimit_Returns429()
    {
        HttpStatusCode? lastStatus = null;

        for (int i = 0; i < 12; i++)
        {
            var response = await api.Client.PostAsJsonAsync("auth/login", new
            {
                email    = "ratelimit@test.com",
                password = "attempt"
            });
            lastStatus = response.StatusCode;

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                break;
        }

        Assert.Equal(HttpStatusCode.TooManyRequests, lastStatus);
    }
}

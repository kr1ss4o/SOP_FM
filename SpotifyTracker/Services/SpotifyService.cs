using SpotifyTracker.Services;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyTracker.Services
{
    public class SpotifyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SpotifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Artist>?> GetTopArtistsAsync(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://api.spotify.com/v1/me/top/artists?limit=5");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TopArtistsResponse>(json);

            return data?.Items;
        }
    }

    public class TopArtistsResponse
    {
        [JsonPropertyName("items")]
        public List<Artist>? Items { get; set; }
    }

    public class Artist
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("images")]
        public List<Image>? Images { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls? ExternalUrls { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class ExternalUrls
    {
        [JsonPropertyName("spotify")]
        public string? Spotify { get; set; }
    }
}
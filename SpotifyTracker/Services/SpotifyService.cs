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

        public async Task<List<Artist>?> GetTopArtistsAsync(string accessToken, string timeRange = "medium_term")
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"https://api.spotify.com/v1/me/top/artists?limit=5&time_range={timeRange}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TopArtistsResponse>(json);

            return data?.Items;
        }

        public async Task<List<Track>?> GetTopTracksAsync(string accessToken, string timeRange = "medium_term")
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"https://api.spotify.com/v1/me/top/tracks?limit=5&time_range={timeRange}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TopTracksResponse>(json);

            return data?.Items;
        }

        public async Task<List<string>?> GetTopGenresAsync(string accessToken, string timeRange = "medium_term")
        {
            var topArtists = await GetTopArtistsAsync(accessToken, timeRange);

            if (topArtists == null) return null;

            var genres = topArtists.SelectMany(a => a.Genres ?? new List<string>())
                                   .GroupBy(g => g)
                                   .OrderByDescending(g => g.Count())
                                   .Take(5)
                                   .Select(g => g.Key)
                                   .ToList();

            return genres;
        }
    }
    //
    // CLASSES
    //
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

        [JsonPropertyName("genres")]
        public List<string>? Genres { get; set; }
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
    public class TopTracksResponse
    {
        [JsonPropertyName("items")]
        public List<Track>? Items { get; set; }
    }

    public class Track
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("album")]
        public Album? Album { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls? ExternalUrls { get; set; }

        [JsonPropertyName("artists")]
        public List<Artist>? Artists { get; set; }
    }

    public class Album
    {
        [JsonPropertyName("images")]
        public List<Image>? Images { get; set; }
    }

}
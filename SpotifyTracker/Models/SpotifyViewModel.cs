using SpotifyTracker.Services;

namespace SpotifyTracker.Models
{
    public class SpotifyViewModel
    {
        public List<Artist>? TopArtists { get; set; }
        public List<string>? TopGenres { get; set; }
        public List<Track>? TopTracks { get; set; }
    }
}
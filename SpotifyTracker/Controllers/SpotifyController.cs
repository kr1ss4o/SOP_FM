using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyTracker.Models;
using SpotifyTracker.Services;
using SpotifyTracker.Services;

namespace SpotifyTracker.Controllers
{
    [Authorize]
    public class SpotifyController : Controller
    {
        private readonly SpotifyService _spotifyService;

        public SpotifyController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        public async Task<IActionResult> TopArtists(string timeRange = "medium_term")
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var topArtists = await _spotifyService.GetTopArtistsAsync(accessToken, timeRange);
            var topGenres = await _spotifyService.GetTopGenresAsync(accessToken, timeRange);
            var topTracks = await _spotifyService.GetTopTracksAsync(accessToken, timeRange);

            var viewModel = new SpotifyViewModel
            {
                TopArtists = topArtists,
                TopGenres = topGenres,
                TopTracks = topTracks
            };

            return View(viewModel);
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
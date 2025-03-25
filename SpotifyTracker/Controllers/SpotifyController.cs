using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> TopArtists()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var artists = await _spotifyService.GetTopArtistsAsync(accessToken);

            return View(artists);
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
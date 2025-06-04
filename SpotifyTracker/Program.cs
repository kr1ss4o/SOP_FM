using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using SpotifyTracker.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<SpotifyService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Spotify";
})
.AddCookie()
.AddOAuth("Spotify", options =>
{
    options.ClientId = "e0ef1fa1d9e4422c99ce72b43bccc4c0";
    options.ClientSecret = "1154fab833ee4c689e6f1c5094b3ed6c";
    options.CallbackPath = "/auth/callback";

    options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
    options.TokenEndpoint = "https://accounts.spotify.com/api/token";
    options.UserInformationEndpoint = "https://api.spotify.com/v1/me";

    options.Scope.Add("user-top-read");
    options.Scope.Add("user-read-recently-played");
    options.Scope.Add("playlist-read-private");
    options.Scope.Add("playlist-read-collaborative");

    options.SaveTokens = true;

    options.ClaimActions.MapJsonKey("urn:spotify:id", "id");
    options.ClaimActions.MapJsonKey("urn:spotify:display_name", "display_name");

    options.Events.OnCreatingTicket = async context =>
    {
        var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);

        var response = await context.Backchannel.SendAsync(request);
        var user = await response.Content.ReadFromJsonAsync<JsonElement>();

        context.RunClaimActions(user);
    };
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();
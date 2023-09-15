using SongsProvider.Spotify.Interfaces;
using SongsProvider.UrlService;
using System.Net.Http.Headers;

namespace SongsProvider.Spotify
{
    public class SpotifyApiService : ISpotifyApiService
    {
        private const string _apiUrl = "https://api.spotify.com/v1";
        private readonly Token _token;

        public SpotifyApiService(Token token) {
            _token = token;
        }

        public async Task<string> GetSongString(string playListId, int offset) 
        { 
            var url = GetApiUrl(playListId, offset);
            return await SendRequest(url);
        }

        private string GetApiUrl(string playListId, int offset)
        {
            var limit = 1;

            var fields = "items(track(artists(name), name, preview_url, external_urls))";
            var urlBuilder = new UrlBuilder($"{_apiUrl}/playlists/{playListId}/tracks");
            urlBuilder.AddParameter("limit", limit.ToString());
            urlBuilder.AddParameter("offset", offset.ToString());
            urlBuilder.AddParameter("fields", fields);
            var url = urlBuilder.GetUrl();

            return url;
        }

        private async Task<string> SendRequest(string url)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_token.TokenType, _token.AccessToken);
            var result = await httpClient.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();

            return json;
        }
    }
}

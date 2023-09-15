using GameBussinesLogic.Songs.Models;
using Newtonsoft.Json;

namespace SongsProvider.Spotify.Models
{
    public class SpotifyExtendedUrls : IExtendedUrls
    {
        [JsonProperty("spotify")]
        public string? SongLink { get; set; }
    }
}

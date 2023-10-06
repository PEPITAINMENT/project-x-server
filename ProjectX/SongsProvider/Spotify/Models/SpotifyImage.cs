using GameBussinesLogic.Songs.Models;
using Newtonsoft.Json;

namespace SongsProvider.Spotify.Models
{
    public class SpotifySpotifyImage : IImage
    {
        [JsonProperty("url")]
        public string? Url { get; set; }
    }
}

using GameBussinesLogic.Songs.Models;
using Newtonsoft.Json;

namespace SongsProvider.Spotify.Models
{
    public class SpotifySong: ISong
    {
        [JsonProperty("external_urls")]
        public SpotifyExtendedUrls? ExtendedUrls { private get; set; }

        [JsonProperty("artists")]
        public IEnumerable<SpotifyArtist>? Artists { private get; set; }

        [JsonProperty("name")]
        public string? Title { get; set; }

        [JsonProperty("preview_url")]
        public string? PreviewUrl { get; set; }

        public IExtendedUrls GetExtendedUrls()
        {
            return ExtendedUrls;
        }

        public IEnumerable<IArtist>? GetArtists()
        {
            return Artists;
        }
    }
}

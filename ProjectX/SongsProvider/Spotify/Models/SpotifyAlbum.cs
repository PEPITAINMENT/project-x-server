using GameBussinesLogic.Songs.Models;
using Newtonsoft.Json;
using SongsProvider.Spotify.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsProvider.Spotify.Models
{
    public class SpotifyAlbum
    {
        [JsonProperty("images")]
        public IEnumerable<SpotifySpotifyImage> Images { get; set; }
    }
}

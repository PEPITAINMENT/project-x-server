using GameBussinesLogic.Songs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsProvider.Spotify.Models
{
    public class SpotifyArtist : IArtist
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

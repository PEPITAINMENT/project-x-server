using Newtonsoft.Json.Linq;
using SongsProvider.Spotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsProvider.Converter
{
    public class JsonSongConverted
    {
        public static SpotifySong Convert(string json)
        {
            var jsonResponse = JObject.Parse(json);
            var items = (JArray)jsonResponse["items"];
            var track = items.FirstOrDefault()?["track"];
            if (track != null)
            {
                var song = track.ToObject<SpotifySong>();
                return song;
            }

            return null;
        }
    }
}

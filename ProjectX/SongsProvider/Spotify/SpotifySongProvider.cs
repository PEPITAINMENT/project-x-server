using GameBussinesLogic.Songs.Models;
using SongsProvider.Converter;
using SongsProvider.Spotify.Interfaces;

namespace SongsProvider.Spotify
{
    public class SpotifySongProvider : SongProvider
    {
        private readonly ISpotifyApiService _spotifyApiService;

        public SpotifySongProvider(ISpotifyApiService spotifyApiService) : base()
        {
            _spotifyApiService = spotifyApiService;
        }

        protected override async Task<ISong> GetSong(string playlist)
        {
            var song = await _spotifyApiService.GetSongString(playlist, _offset);

            return JsonSongConverted.Convert(song);
        }
    }
}

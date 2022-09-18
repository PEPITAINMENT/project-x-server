using SongBussinsLogic.Models;
using SongBussinsLogic.Services.Storage;
using System.Collections.Generic;

namespace SongBussinsLogic.Services.Request
{
    public abstract class SongProviderBase : ISongProviderBase
    {
        private readonly ISongStorageService _storageService;
        protected readonly int _limit;

        public SongProviderBase(ISongStorageService songStorageService,
            int limit) { 
            _storageService = songStorageService;
            _limit = limit;
        }

        public ISong GetSong(string key) {
            var song = _storageService.GetSong(key);
            
            if (song == null) {
                var songs = RequestSongs();
                _storageService.StoreSongs(key, new Queue<ISong>(songs));

                song = _storageService.GetSong(key);
            }

            return song;
        }

        protected abstract IEnumerable<ISong> RequestSongs();
    }
}

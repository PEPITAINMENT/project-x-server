using SongBussinsLogic.Models;
using System.Collections.Generic;

namespace SongBussinsLogic.Services.Storage
{
    public abstract class SongStorageServiceBase : ISongStorageService
    {
        public abstract Queue<ISong> GetSongs(string key);
        public abstract void StoreSongs(string key, Queue<ISong> songs);
        public virtual ISong GetSong(string key) {
            var songs = GetSongs(key);

            var song = songs?.Dequeue();

            if (song != null) {
                StoreSongs(key, songs);
            }

            return song;
        }
    }
}

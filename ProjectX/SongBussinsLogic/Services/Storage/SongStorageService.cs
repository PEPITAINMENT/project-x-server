using SongBussinsLogic.Models;
using System.Collections.Generic;

namespace SongBussinsLogic.Services.Storage
{
    public interface ISongStorageService
    {
        Queue<ISong> GetSongs(string key);
        void StoreSongs(string key, Queue<ISong> songs);
        ISong GetSong(string key);
    }
}

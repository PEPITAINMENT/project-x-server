using Microsoft.Extensions.Caching.Memory;
using SongBussinsLogic.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SongBussinsLogic.Services.Storage
{
    public class SongStorageService : SongStorageServiceBase
    {
        private readonly IMemoryCache _memoryCache;

        public SongStorageService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public override Queue<ISong> GetSongs(string key)
        {
            _memoryCache.TryGetValue(key, out Queue<ISong> song);

            return song;
        }
        public override void StoreSongs(string key, Queue<ISong> songs)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key is empty or null");
            }

            if (songs == null)
            {
                throw new ArgumentException("Songs is null");
            }

            _memoryCache.Set(key, songs);
        }
    }
}

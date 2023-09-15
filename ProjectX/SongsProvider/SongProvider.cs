using GameBussinesLogic.Models;
using GameBussinesLogic.Songs.Models;

namespace SongsProvider
{
    public abstract class SongProvider : ISongProvider
    {
        protected int _offset = 0;

        protected abstract Task<ISong> GetSong(string playlist);

        public async Task<ISong> GetNextSong(string playlist)
        {
            ISong song;
            do
            {
                song = await GetSong(playlist);
                _offset++;

                if (song == null)
                    return null;

            } while (string.IsNullOrEmpty(song.PreviewUrl));


            return song;
        }
    }
}
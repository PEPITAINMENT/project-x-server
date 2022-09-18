using SongBussinsLogic.Models;

namespace SongBussinsLogic.Services.Request
{
    public interface ISongProviderBase
    {
        ISong GetSong(string key);
    }
}

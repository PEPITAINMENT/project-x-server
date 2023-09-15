namespace SongsProvider.Spotify.Interfaces
{
    public interface ISpotifyApiService
    {
        Task<string> GetSongString(string playListId, int offset);
    }
}

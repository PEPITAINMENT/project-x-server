namespace ProjextX.Services.Interfaces
{
    public interface IUserGamesService
    {
        bool IsUserHasActiveGame(string userIdentifier);
        string GetUserActiveGameId(string userIdentifier);
        void AddUserToGame(string userIdentifier, string gameId);
        int GetUsersInGroup(string gameId);
        void Remove(string gameId);
    }
}

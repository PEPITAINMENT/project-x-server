namespace ProjextX.Services.Interfaces
{
    public interface IGameStatusService
    {
        bool IsGameCanStarted(string gameId, int usersInGame);
        void AddReadyStatus(string gameId);
        void Remove(string gameId);

        void RunGame(string gameId);
        bool IsGameRunned(string gameId);
    }
}

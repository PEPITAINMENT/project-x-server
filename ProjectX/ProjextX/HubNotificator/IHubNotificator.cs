namespace Server.HubNotificator
{
    public interface IHubNotificator
    {
        void RunGame(string gameId);

        void StopGame(string gameId);
    }
}

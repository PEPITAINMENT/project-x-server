using System.Threading.Tasks;

namespace Server.HubNotificator
{
    public interface IHubNotificator
    {
        Task RunGame(string gameId, string playList);

        void StopGame(string gameId);
    }
}

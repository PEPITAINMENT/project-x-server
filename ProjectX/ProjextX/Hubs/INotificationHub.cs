using System.Threading.Tasks;

namespace Server.HubNotificator
{
    public interface IHubNotificator 
    {
        Task RunGame(string roomId);

        void StopGame(string roomId);
    }
}
using GameBussinesLogic.Runner;
using Microsoft.AspNetCore.SignalR;
using ProjextX.Hubs;
using System.Threading.Tasks;

namespace Server.HubNotificator
{
    public class HubNotificator : IHubNotificator
    {
        private readonly IHubContext<GameSessionHub> _hub;
        private readonly IGameRunner _gameRunner;

        public HubNotificator(
            IHubContext<GameSessionHub> hub,
            IGameRunner gameRunner)
        {
            _hub = hub;
            _gameRunner = gameRunner;

            _gameRunner.OnSongChange += OnSongChange;
        }

        public async Task RunGame(string gameId, string playList)
        {
            await _gameRunner.RunGame(gameId, playList);
        }

        public void StopGame(string gameId) {
            _gameRunner.StopGame(gameId);
        }

        private void OnSongChange(string gameId, string songLink)
        {
            _hub.Clients.Groups(gameId).SendAsync("onSongPreviewLinkChanged", songLink);
        }
    }
}

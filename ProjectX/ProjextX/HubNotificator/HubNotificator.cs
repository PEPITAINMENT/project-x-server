using GameBussinesLogic.Runner;
using GameBussinesLogic.Songs.Models;
using Microsoft.AspNetCore.SignalR;
using ProjextX.Hubs;
using System.Linq;
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
            _gameRunner.OnAnswerProvide += OnAnswerProvide;
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

        private void OnAnswerProvide(string gameId, ISong song)
        {
            var songPreview = new SongPreview()
            {
                Title = song.Title,
                Artists = string.Join(',', song.GetArtists()?.Select(x => x.Name)),
                ImageUrl = song.GetImageUrl(),
            };
            _hub.Clients.Groups(gameId).SendAsync("onAnswerShow", songPreview);
        }

        class SongPreview { 
            public string Title { get; set; }
            public string Artists { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}

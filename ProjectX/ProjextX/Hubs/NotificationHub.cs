using GameBussinesLogic.IServices;
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
        private readonly IRoomService _roomService;

        public HubNotificator(
            IHubContext<GameSessionHub> hub,
            IRoomService roomService)
        {
            _hub = hub;
            _roomService = roomService;

            _roomService.OnSongChange += OnSongChange;
            _roomService.OnAnswerProvide += OnAnswerProvide;
        }

        public async Task RunGame(string roomId)
        {
            await _roomService.Run(roomId);
        }

        public void StopGame(string roomId)
        {
            _roomService.Stop(roomId);
        }

        private void OnSongChange(string roomId, ISong song)
        {
            _hub.Clients.Groups(roomId).SendAsync("onSongPreviewLinkChanged", song.PreviewUrl);
        }

        private void OnAnswerProvide(string roomId, ISong song)
        {
            var songPreview = new SongPreview()
            {
                Title = song.Title,
                Artists = string.Join(',', song.GetArtists()?.Select(x => x.Name)),
                ImageUrl = song.GetImageUrl(),
            };
            _hub.Clients.Groups(roomId).SendAsync("onAnswerShow", songPreview);
        }

        class SongPreview
        {
            public string Title { get; set; }
            public string Artists { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
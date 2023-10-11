using GameBussinesLogic.IServices;
using GameBussinesLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ProjextX.Services.Interfaces;
using Server.HubNotificator;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ProjextX.Hubs
{
    [Authorize]
    public class GameSessionHub : Hub
    {
        private readonly IRoomService _roomService;
        private readonly IHubNotificator _hubNotificator;
        public GameSessionHub(
            IRoomService roomService,
            IHubNotificator hubNotificator
            ) {
            _roomService = roomService;
            _hubNotificator = hubNotificator;
        }

        public async Task Join(string roomId) {
            var name = Context.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
            var player = new Player()
            {
                Id = Context.User.Identity.Name,
                Name = name,
            };
            var roomInfo = _roomService.GetRoomInfoModel(roomId);

            await this.Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            _roomService.Join(roomId, player);

            await this.Clients.OthersInGroup(roomId).SendAsync("onUserJoin", player);
            await this.Clients.Caller.SendAsync("onJoin", roomInfo);
        }

        public async Task ChangePlaylist(string roomId, string playlist) {
            var userId = Context.User.Identity.Name;
            _roomService.UpdatePlaylist(roomId, userId, playlist);
            await this.Clients.Group(roomId).SendAsync("onPlaylistChange", playlist);
        }

        public async Task RunGame(string roomId) {
            await _hubNotificator.RunGame(roomId);
        }

        public async Task Guess(string roomId, string message) {
            var playerId = Context.User.Identity.Name;
            var points = _roomService.Guess(roomId, playerId, message);
            var player = _roomService.GetPlayer(roomId, playerId);
            if (points != 0) {
                await this.Clients.Group(roomId).SendAsync("onUserAnswer", player);
            }
        }
    }
}

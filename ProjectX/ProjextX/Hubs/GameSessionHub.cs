using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ProjextX.Services.Interfaces;
using Server.HubNotificator;
using System;
using System.Threading.Tasks;

namespace ProjextX.Hubs
{
    [Authorize]
    public class GameSessionHub : Hub
    {
        private readonly int maxUsersCount = 2;
        private readonly IUserGamesService _userGamesService;
        private readonly IHubNotificator _hubNotificator;
        private readonly IGameStatusService _gameStatusService;
        public GameSessionHub(IUserGamesService userGamesService,
            IHubNotificator hubNotificator,
            IGameStatusService gameStatusService) {
            _userGamesService = userGamesService;
            _hubNotificator = hubNotificator;
            _gameStatusService = gameStatusService;
        }

        public async Task Join(string gameId) {
            if (_userGamesService.GetUsersInGroup(gameId) >= maxUsersCount) {
                await this.Clients.Caller.SendAsync("onNoSlots");
                return;
            }

            if (_userGamesService.IsUserHasActiveGame(Context.ConnectionId)) {
                var activeGameId = _userGamesService.GetUserActiveGameId(Context.ConnectionId);
                await this.Groups.RemoveFromGroupAsync(Context.ConnectionId, activeGameId);
                ClearDateAssociatedWithUser(Context.ConnectionId);
            }

            _userGamesService.AddUserToGame(Context.ConnectionId, gameId);
            await this.Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await this.Clients.OthersInGroup(gameId).SendAsync("onUserJoin", Context.ConnectionId);
        }

        public async Task RunGame(string gameId, string playList) {
            await _hubNotificator.RunGame(gameId, playList);
        }

        public async Task Guess(string gameId, string message) {
            //compare message and update game state
            await this.Clients.Group(gameId).SendAsync("updateGameState", "GAME STATE");
        }

        public void SetReadyStatus(string gameId) {
            _gameStatusService.AddReadyStatus(gameId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_userGamesService.IsUserHasActiveGame(Context.ConnectionId))
            {
                var activeGameId = _userGamesService.GetUserActiveGameId(Context.ConnectionId);
                this.Groups.RemoveFromGroupAsync(Context.ConnectionId, activeGameId).ConfigureAwait(false);
                ClearDateAssociatedWithUser(activeGameId);

                if (_userGamesService.GetUsersInGroup(activeGameId) == 0) {
                    _hubNotificator.StopGame(activeGameId);
                }
            }
            return base.OnDisconnectedAsync(exception);
        }

        public void ClearDateAssociatedWithUser(string userId) {
            _userGamesService.Remove(userId);
        }
    }
}

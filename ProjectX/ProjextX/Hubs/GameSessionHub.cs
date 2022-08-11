using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ProjextX.Repositories;
using ProjextX.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProjextX.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly int maxUsersCount = 2;
        private readonly IUserGamesService _userGamesService;
        private readonly IGameRepository _gameRepository;
        private readonly IGameHubNotificator _gameHubNotificator;
        private readonly IGameStatusService _gameStatusService;
        public GameSessionHub(IUserGamesService userGamesService,
            IGameRepository gameRepository,
            IGameHubNotificator gameHubNotificator,
            IGameStatusService gameStatusService) {
            _userGamesService = userGamesService;
            _gameRepository = gameRepository;
            _gameHubNotificator = gameHubNotificator;
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
                ClearDateAssociatedWithGame(activeGameId);
            }

            _userGamesService.AddUserToGame(Context.ConnectionId, gameId);
            await this.Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await this.Clients.OthersInGroup(gameId).SendAsync("onUserJoin", Context.ConnectionId);
        }

        public void RunGame(string gameId) {
            var game = _gameRepository.GetGame(gameId);
            if (game != null)
            {
                _gameHubNotificator.RunGame(game);
            }
        }

        public async Task Guess(string gameId, string message) {
            //compare message and update game state

            await this.Clients.Group(gameId).SendAsync("updateGameState", "GAME STATE");
        }

        public void SetReadyStatus(string gameId) {
            _gameStatusService.AddReadyStatus(gameId);
            var usersInGroup = _userGamesService.GetUsersInGroup(gameId);
            if (_gameStatusService.IsGameCanStarted(gameId, usersInGroup)) {
                RunGame(gameId);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_userGamesService.IsUserHasActiveGame(Context.ConnectionId))
            {
                var activeGameId = _userGamesService.GetUserActiveGameId(Context.ConnectionId);
                this.Groups.RemoveFromGroupAsync(Context.ConnectionId, activeGameId).ConfigureAwait(false);
                ClearDateAssociatedWithGame(activeGameId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public void ClearDateAssociatedWithGame(string gameId) {
            _userGamesService.Remove(gameId);
            _gameStatusService.Remove(gameId);
        }
    }
}

using ProjextX.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ProjextX.Services
{
    public class UserGamesService : IUserGamesService
    {
        private readonly Dictionary<string, string> _userGames = new Dictionary<string, string>();

        public bool IsUserHasActiveGame(string userIdentifier) {
            return _userGames.ContainsKey(userIdentifier);
        }

        public string GetUserActiveGameId(string userIdentifier) {
            if (_userGames.TryGetValue(userIdentifier, out var gameId)) {
                return gameId;
            }

            return string.Empty;
        }

        public void AddUserToGame(string userIdentifier, string gameId) {
            if (!_userGames.ContainsKey(userIdentifier)) {
                _userGames.Add(userIdentifier, gameId);
            }
        }

        public int GetUsersInGroup(string gameId) {
            return _userGames.Values.Count(element => element == gameId);
        }

        public void Remove(string gameId) {
            _userGames.Remove(gameId);
        }
    }
}

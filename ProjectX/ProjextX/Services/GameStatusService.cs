using ProjextX.Services.Interfaces;
using System.Collections.Generic;

namespace ProjextX.Services
{
    public class GameStatusService : IGameStatusService
    {
        private readonly Dictionary<string, int> _readyStatuses = new Dictionary<string, int>();
        private readonly HashSet<string> _activeGames = new HashSet<string>();

        public bool IsGameCanStarted(string gameId, int usersInGame) {
            if (_readyStatuses.TryGetValue(gameId, out var readyStatuses)) {
                return readyStatuses >= (usersInGame / 2);
            }
            return false;
        }

        public void RunGame(string gameId) {
            if (_activeGames.Contains(gameId)) {
                return;
            }

            _activeGames.Add(gameId);   
        }

        public bool IsGameRunned(string gameId)
        {
            return _activeGames.Contains(gameId);
        }

        public void AddReadyStatus(string gameId) {
            if (_readyStatuses.TryGetValue(gameId, out var readyStatuses))
            {
                readyStatuses += 1;
                _readyStatuses.Add(gameId, readyStatuses);
                return;
            }

            var initValue = 1;
            _readyStatuses.Add(gameId, initValue);
        }

        public void Remove(string gameId)
        {
            _readyStatuses.Remove(gameId);
        }
    }
}

using ProjextX.Services.Interfaces;
using System.Collections.Generic;

namespace ProjextX.Services
{
    public class GameStatusService : IGameStatusService
    {
        private readonly Dictionary<string, int> _readyStatuses = new Dictionary<string, int>();

        public bool IsGameCanStarted(string gameId, int usersInGame) {
            if (_readyStatuses.TryGetValue(gameId, out var readyStatuses)) {
                return readyStatuses >= (usersInGame / 2);
            }
            return false;
        }

        public void AddReadyStatus(string gameId) {
            if (_readyStatuses.TryGetValue(gameId, out var readyStatuses))
            {
                _readyStatuses.Add(gameId, readyStatuses += 1);
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameBussinesLogic.Runner
{
    public class GameRunner : IGameRunner
    {
        public event Action<string, string> OnSongChange;

        private readonly Dictionary<string, CancellationTokenSource> _tokens 
            = new Dictionary<string, CancellationTokenSource>();
        private readonly int _songDelaySeconds;

        public GameRunner(int songDelaySeconds) {
            _songDelaySeconds = songDelaySeconds;
        }

        public void RunGame(string gameId) {
            var cancelationTokenSource = new CancellationTokenSource();
            _tokens.Add(gameId, cancelationTokenSource);
            Task.Factory.StartNew(() =>
            {
                var list = new List<string>() { "One", "Two", "Three", "Four" };
                foreach (var newSongLink in list) {
                    OnSongChange?.Invoke(gameId, newSongLink);
                    Thread.Sleep(TimeSpan.FromSeconds(_songDelaySeconds));
                }
            });
        }

        public void StopGame(string gameId) {
            if (_tokens.TryGetValue(gameId, out var token)) {
                token.Cancel();
            }
        }
    }
}

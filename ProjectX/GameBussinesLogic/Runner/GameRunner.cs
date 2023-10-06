using GameBussinesLogic.Models;
using GameBussinesLogic.Songs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameBussinesLogic.Runner
{
    public class GameRunner : IGameRunner
    {
        public event Action<string, string> OnSongChange;
        public event Action<string, ISong> OnAnswerProvide;

        private readonly Dictionary<string, CancellationTokenSource> _tokens 
            = new Dictionary<string, CancellationTokenSource>();
        private readonly int _songDelaySeconds;
        private readonly int _resultsDelaySeconds;
        private readonly ISongProvider _songProvider;

        public GameRunner(int songDelaySeconds, int resultsDelaySeconds, ISongProvider songProvider) {
            _songDelaySeconds = songDelaySeconds;
            _resultsDelaySeconds = resultsDelaySeconds;
            _songProvider = songProvider;
        }

        public async Task RunGame(string gameId, string playList) {
            var cancelationTokenSource = new CancellationTokenSource();
            _tokens.Add(gameId, cancelationTokenSource);
            await Task.Factory.StartNew(async () =>
            {
                ISong song;
                do {
                    song = await _songProvider.GetNextSong(playList);

                    if (song != null) {
                        OnSongChange?.Invoke(gameId, song.PreviewUrl);
                        Thread.Sleep(TimeSpan.FromSeconds(_songDelaySeconds));
                        OnAnswerProvide?.Invoke(gameId, song);
                        Thread.Sleep(TimeSpan.FromSeconds(_resultsDelaySeconds));
                    }
                } while(song != null);
            });
        }

        public void StopGame(string gameId) {
            if (_tokens.TryGetValue(gameId, out var token)) {
                token.Cancel();
            }
        }
    }
}

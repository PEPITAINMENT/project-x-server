using GameBussinesLogic.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GameBussinesLogic.Songs.Models;
using System;

namespace GameBussinesLogic.Models
{
    public class Room : BaseModel
    {
        public event Action<string, ISong> OnSongChange;
        public event Action<string, ISong> OnAnswerProvide;
        public event Action OnEnded;

        public GameStatus Status { get; set; } = GameStatus.Waiting;
        public string PlaylistId { get; set; }
        public string Name { get; set; }
        public string AdminId { get; set; }
        public ISong LastSong { get; set; }
        public IList<Player> Players { get; set; } = new List<Player>();

        private CancellationTokenSource _cancellationTokenSource;

        public async Task RunGame(ISongProvider songProvider)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            var task = await Task.Factory.StartNew(async () =>
            {
                Status = GameStatus.Running;
                ISong song;
                do
                {
                    token.ThrowIfCancellationRequested();
                    song = await songProvider.GetNextSong(PlaylistId);

                    if (song != null)
                    {
                        LastSong = song;
                        OnSongChange?.Invoke(Id, song);
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                        OnAnswerProvide?.Invoke(Id, song);
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                    }
                } while (song != null);
                Status = GameStatus.Ended;
                OnEnded?.Invoke();
                OnSongChange = null;
                OnAnswerProvide = null;
                OnEnded = null;
            }, token);
        }

        public void StopGame()
        {
            _cancellationTokenSource.Cancel();
            Status = GameStatus.Ended;
        }
    }
}

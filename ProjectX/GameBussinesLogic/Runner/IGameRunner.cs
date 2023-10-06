using GameBussinesLogic.Songs.Models;
using System;
using System.Threading.Tasks;

namespace GameBussinesLogic.Runner
{
    public interface IGameRunner
    {
        event Action<string, string> OnSongChange;
        event Action<string, ISong> OnAnswerProvide;
        Task RunGame(string gameId, string playList);
        void StopGame(string gameId);
    }
}

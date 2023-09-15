using System;
using System.Threading.Tasks;

namespace GameBussinesLogic.Runner
{
    public interface IGameRunner
    {
        event Action<string, string> OnSongChange;
        Task RunGame(string gameId, string playList);
        void StopGame(string gameId);
    }
}

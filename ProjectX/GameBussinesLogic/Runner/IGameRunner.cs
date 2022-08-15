using System;

namespace GameBussinesLogic.Runner
{
    public interface IGameRunner
    {
        event Action<string, string> OnSongChange;
        void RunGame(string gameId);
        void StopGame(string gameId);
    }
}

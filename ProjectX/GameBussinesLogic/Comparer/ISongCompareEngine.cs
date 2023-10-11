using GameBussinesLogic.Songs.Models;

namespace GameBussinesLogic.Comparer
{
    public interface ISongCompareEngine
    {
        GuessSongProperties Compare(string userGuess, ISong song);
    }
}

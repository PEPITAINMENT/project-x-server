using GameBussinesLogic.Comparer;
using GameBussinesLogic.IServices;
using GameBussinesLogic.Models;
using GameBussinesLogic.Songs.Models;

namespace GameBussinesLogic.Services
{
    public class GuessService : IGuessService
    {
        private const int POINTS = 5;
        private readonly ISongCompareEngine _songCompareEngine;
        public GuessService(
            ISongCompareEngine songCompareEngine
            ) {
            _songCompareEngine = songCompareEngine;
        }

        public int Guess(ISong song, Player player, string guess) {
            var result = _songCompareEngine.Compare(guess, song);
            var points = GetNewPoints(player.LastGuess, result);
            player.LastGuess |= result;
            player.NewPoints += points;

            return points;
        }

        private int GetNewPoints(GuessSongProperties lastGuess, GuessSongProperties currentGuess) {
            if (currentGuess.HasFlag(GuessSongProperties.Title) 
                && !lastGuess.HasFlag(GuessSongProperties.Title)) {
                return POINTS;
            }

            if (currentGuess.HasFlag(GuessSongProperties.Author)
                && !lastGuess.HasFlag(GuessSongProperties.Author))
            {
                return POINTS;
            }

            return 0;
        }
    }
}

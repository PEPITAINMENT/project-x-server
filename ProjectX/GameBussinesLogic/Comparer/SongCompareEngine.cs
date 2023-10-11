using GameBussinesLogic.Comparer.FuzzyComaprer;
using GameBussinesLogic.Songs.Models;
using System;

namespace GameBussinesLogic.Comparer
{
    [Flags]
    public enum GuessSongProperties
    { 
        None,
        Author,
        Title,
    }

    public class SongCompareEngine : ISongCompareEngine
    {
        private readonly IFuzzyComparer _comparer;

        public SongCompareEngine(IFuzzyComparer fuzzyComparer) { 
            _comparer = fuzzyComparer;
        }

        public GuessSongProperties Compare(string userGuess, ISong song) {
            var guessPropirties = GuessSongProperties.None;
            guessPropirties |= GetGuessProperty(userGuess, song.Title, GuessSongProperties.Title);

            var artists = song.GetArtists();
            foreach(var artist in artists )
            {
                guessPropirties |= GetGuessProperty(userGuess, artist.Name, GuessSongProperties.Author);
            }
           

            return guessPropirties;
        }

        private GuessSongProperties GetGuessProperty(string userGuess, 
            string comparingProperty, GuessSongProperties property) {
            return _comparer.IsMatch(userGuess, comparingProperty)
                ? property : GuessSongProperties.None;
        }
    }
}

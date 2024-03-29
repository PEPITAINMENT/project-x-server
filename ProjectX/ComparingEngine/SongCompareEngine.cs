﻿using ComparingEngine.FuzzyComaprer;
using SongBussinsLogic.Models;
using System;

namespace ComparingEngine
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
            guessPropirties &= GetGuessProperty(userGuess, song.Title, GuessSongProperties.Title);
            guessPropirties &= GetGuessProperty(userGuess, song.Author, GuessSongProperties.Author);

            return guessPropirties;
        }

        private GuessSongProperties GetGuessProperty(string userGuess, 
            string comparingProperty, GuessSongProperties property) {
            return _comparer.IsMatch(userGuess, comparingProperty)
                ? GuessSongProperties.None : property;
        }
    }
}

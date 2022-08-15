using System;
using FuzzySharp;

namespace ComparingEngine.FuzzyComaprer
{
    public class FuzzyComparer : IFuzzyComparer
    {
        private readonly int _matchPercent;
        public FuzzyComparer(int matchPercent)
        {
            if (matchPercent < 0 || matchPercent > 100)
            {
                throw new ArgumentException("Matching percent should be between 0 and 100");
            }

            _matchPercent = matchPercent;
        }
        public bool IsMatch(string main, string matching)
        {
            var ration = Fuzz.PartialRatio(matching, main);

            return ration >= _matchPercent;
        }
    }
}

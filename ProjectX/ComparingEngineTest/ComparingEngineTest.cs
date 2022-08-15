using ComparingEngine.FuzzyComaprer;
using NUnit.Framework;
using System;

namespace ComparingEngineTest
{
    public class ComparingEngineTest
    {
        [TestCase(-10)]
        [TestCase(101)]
        public void Invalid_Percent_Parameter_Throws_Exception(int matchingPercent)
        {
            Assert.Throws<ArgumentException>(() => new FuzzyComparer(matchingPercent));
        }

        [TestCase("Somelikeyou - Nilleto", "Nilleto", true)]
        [TestCase("Somelikeyou - Nilleto", "Nileto", true)]
        [TestCase("Somelikeyou - Nilleto", "Nilrefezo", false)]
        [TestCase("Somelikeyou - Nilleto", "open", false)]
        public void IsMatch_Is_Match_Partial(string main, string matching, bool expected)
        {
            var percent = 80;
            var comparingEngine = new FuzzyComparer(percent);
            var result = comparingEngine.IsMatch(main, matching);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
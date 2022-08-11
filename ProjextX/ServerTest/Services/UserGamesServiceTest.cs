using NUnit.Framework;
using ProjextX.Services;

namespace ServerTest.Services
{
    public class UserGamesServiceTest
    {
        private UserGamesService _userGamesService;

        [SetUp]
        public void Setup()
        {
            _userGamesService = new UserGamesService();

        }

        [TestCase("user1", true)]
        [TestCase("user2", false)]
        public void IsUserHasActiveGame(string userID, bool expected)
        {
            _userGamesService.AddUserToGame("user1", "gameID");

            var actual = _userGamesService.IsUserHasActiveGame(userID);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("user1", "gameID")]
        [TestCase("user2", "")]
        public void IsUserHasActiveGame(string userID, string expected)
        {
            _userGamesService.AddUserToGame("user1", "gameID");

            var actual = _userGamesService.GetUserActiveGameId(userID);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetUsersInGroup()
        {
            var expectedUserNumber = 2;
            var gameID = "gameID";
            _userGamesService.AddUserToGame("user1", gameID);
            _userGamesService.AddUserToGame("user2", gameID);

            var actual = _userGamesService.GetUsersInGroup(gameID);

            Assert.That(actual, Is.EqualTo(expectedUserNumber));
        }

        [Test]
        public void Remove()
        {
            var gameId = "gameId";
            _userGamesService.AddUserToGame("user1", gameId);

            Assert.DoesNotThrow(() => _userGamesService.Remove(gameId));
        }
    }
}
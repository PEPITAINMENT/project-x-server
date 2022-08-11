using Microsoft.AspNetCore.SignalR;
using ProjextX.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjextX.Repositories
{
    public enum GameStatus { 
        Running,
        Waiting,
    }

    public class Game {
        public string Id { get; set; }
        public string Name { get; set; }

        public GameStatus GameStatus { get; set; }

        public List<string> GetPreviewLinks() {
            return new List<string>() {
                    $"Song1_{Id}",
                    $"Song2_{Id}",
                    $"Song3_{Id}",
                    $"Song4_{Id}",
                    $"Song5_{Id}"
                };
        }
    }

    public class GameRepository : IGameRepository
    {
        private readonly List<Game> _games = new List<Game>() {
            new Game() { Id = "1" },
            new Game() { Id = "2" },
        };

        public Game GetGame(string id) {
            return _games.FirstOrDefault(game => game.Id == id);
        }
    }

    public interface IGameRepository {
        Game GetGame(string id);
    }

    public class GameHubNotificator : IGameHubNotificator
    {
        private readonly IHubContext<GameSessionHub> _hub;
        public GameHubNotificator(IHubContext<GameSessionHub> hub) {
            _hub = hub;
        }

        public void RunGame(Game game) {
            Task.Factory.StartNew(() =>
            {
                var list = game.GetPreviewLinks();
                for (var i = 0; i < 10; i++) {
                    foreach (var element in list)
                    {
                        _hub.Clients.Groups(game.Id).SendAsync("onSongPreviewLinkChanged", element);
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                }
            });
        }
    }

    public interface IGameHubNotificator {
        void RunGame(Game game);
    }
}

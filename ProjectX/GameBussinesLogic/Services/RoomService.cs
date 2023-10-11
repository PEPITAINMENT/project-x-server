using GameBussinesLogic.IServices;
using GameBussinesLogic.Models;
using GameBussinesLogic.Repositories;
using GameBussinesLogic.Songs.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GameBussinesLogic.Services
{
    public class RoomService : IRoomService
    {
        public event Action<string, ISong> OnSongChange;
        public event Action<string, ISong> OnAnswerProvide;

        private readonly ISongProvider _songProvider;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuessService _guessService;
        public RoomService(
            IRoomRepository roomRepository,
            ISongProvider songProvider,
            IGuessService guessService
            ) {
            _songProvider = songProvider;
            _roomRepository = roomRepository;
            _guessService = guessService;
        }

        public string Create(string adminId, string userName) {
            var guid = Guid.NewGuid();
            var room = new Room()
            {
                Id = guid.ToString(),
                AdminId = adminId,
                Name = "Room",
                Status = Enums.GameStatus.Waiting,
            };
            room.Players.Add(new Player() { Id = adminId, Name = userName });
            var rooms = _roomRepository.GetAll();
            var adminRooms = rooms.Where(x => x.AdminId == adminId);
            foreach(var adminRoom in adminRooms) {
                _roomRepository.Remove(adminRoom.Id);
            }

            _roomRepository.Add(room);

           return guid.ToString();
        }

        public Room Get(string roomId)
        {
            return _roomRepository.Get(roomId);
        }

        public RoomInfoModel GetRoomInfoModel(string roomId) {
            var room = _roomRepository.Get(roomId);
            return new RoomInfoModel()
            {
                PlaylistId = room.PlaylistId,
                Name = room.Name,
                Players = room.Players
            };
        }

        public void UpdatePlaylist(string roomId, string playerId, string playListId)
        {
            var room = _roomRepository.Get(roomId);
            if(playerId != room.AdminId)
            {
                throw new Exception("No permissions");
            }

            if (room.Status == Enums.GameStatus.Running) 
            {
                throw new Exception("Room in run state");
            }

            room.PlaylistId = playListId;
        }

        public void Join(string roomId, Player player) {
            var room = _roomRepository.Get(roomId);
            if (room == null)
            {
                throw new Exception("No room");
            }

            if (room.Players.FirstOrDefault(x => x.Id == player.Id) != null)
            {
                return;
            }

            room.Players.Add(player);
        }

        public async Task Run(string roomId)
        {
            var room = _roomRepository.Get(roomId);
            if (room == null)
            {
                throw new Exception("No room");
            }
            room.OnSongChange += (roomId, song) => this.OnSongChange?.Invoke(roomId, song);
            room.OnAnswerProvide += (roomId, song) => this.OnAnswerProvide?.Invoke(roomId, song);
            room.OnEnded += () => {
                this.OnSongChange = null;
                this.OnAnswerProvide = null;
            };
            await room.RunGame(_songProvider);
        }

        public void Stop(string roomId)
        {
            var room = _roomRepository.Get(roomId);
            if (room == null)
            {
                throw new Exception("No room");
            }
            room.OnSongChange -= (roomId, song) => this.OnSongChange?.Invoke(roomId, song);
            room.OnAnswerProvide -= (roomId, song) => this.OnAnswerProvide?.Invoke(roomId, song);

            room.StopGame();
        }

        public int Guess(string roomId, string playerId, string guess)
        {
            var room = _roomRepository.Get(roomId);
            if (room == null)
            {
                throw new Exception("No room");
            }
            var player = room.Players.FirstOrDefault(x => x.Id == playerId);
            var points = _guessService.Guess(room.LastSong, player, guess);

            return points;
        }

        public Player GetPlayer(string roomId, string playerId)
        {
            var room = _roomRepository.Get(roomId);
            if (room == null)
            {
                throw new Exception("No room");
            }
            var player = room.Players.FirstOrDefault(x => x.Id == playerId);

            return player;
        }
    }
}

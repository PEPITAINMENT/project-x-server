using GameBussinesLogic.Models;
using GameBussinesLogic.Songs.Models;
using System;
using System.Threading.Tasks;

namespace GameBussinesLogic.IServices
{
    public interface IRoomService
    {
        public event Action<string, ISong> OnSongChange;
        public event Action<string, ISong> OnAnswerProvide;

        string Create(string adminId, string name);
        Room Get(string roomId);
        void Join(string roomId, Player player);
        Player GetPlayer(string roomId, string playerId);
        RoomInfoModel GetRoomInfoModel(string roomId);
        void UpdatePlaylist(string roomId, string playerId, string playListId);
        int Guess(string roomId, string playerId, string guess);
        Task Run(string roomId);
        void Stop(string roomId);
    }
}

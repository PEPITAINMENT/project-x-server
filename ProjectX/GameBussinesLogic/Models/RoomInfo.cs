using GameBussinesLogic.Enums;
using System.Collections.Generic;

namespace GameBussinesLogic.Models
{
    public class RoomInfoModel
    {
        public GameStatus Status { get; set; } = GameStatus.Waiting;
        public string PlaylistId { get; set; }
        public string Name { get; set; }
        public IList<Player> Players { get; set; } = new List<Player>();
    }
}

using GameBussinesLogic.Enums;

namespace GameBussinesLogic.Models
{
    public class Game : BaseModel
    {
        public string Name { get; set; }
        public GameStatus Status { get; set; }
        public string PlayListId { get; set; }
    }
}

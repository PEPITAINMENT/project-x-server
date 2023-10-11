using GameBussinesLogic.Comparer;

namespace GameBussinesLogic.Models
{
    public class Player : BaseModel
    {
        public string Name { get; set; }
        public int TotalPoints { get; set; }
        public int NewPoints { get; set; }
        public GuessSongProperties LastGuess { get; set; }
    }
}

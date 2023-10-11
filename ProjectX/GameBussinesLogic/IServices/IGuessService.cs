using GameBussinesLogic.Models;
using GameBussinesLogic.Songs.Models;

namespace GameBussinesLogic.IServices
{
    public interface IGuessService
    {
        int Guess(ISong song, Player player, string guess);
    }
}

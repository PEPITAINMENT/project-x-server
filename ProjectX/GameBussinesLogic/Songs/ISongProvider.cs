using GameBussinesLogic.Songs.Models;
using System.Threading.Tasks;

namespace GameBussinesLogic.Models
{
    public interface ISongProvider
    {
        Task<ISong> GetNextSong(string playlist);
    }
}
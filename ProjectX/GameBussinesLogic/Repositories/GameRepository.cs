using GameBussinesLogic.Models;

namespace GameBussinesLogic.Repositories
{
    public class GameRepository : MemoryStoredRepository<Game>, IGameRepository
    {
        protected override void UpdateFields(Game currentEntityState, Game newEntityState)
        {
            currentEntityState.Name = newEntityState.Name;
            currentEntityState.Status = newEntityState.Status;
        }
    }
}

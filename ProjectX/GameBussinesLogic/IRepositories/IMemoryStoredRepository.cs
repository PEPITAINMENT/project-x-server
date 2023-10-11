using GameBussinesLogic.Models;
using System.Collections.Generic;

namespace GameBussinesLogic.Repositories
{
    public interface IMemoryStoredRepository<T> where T : BaseModel
    {
        T Get(string id);

        ICollection<T> GetAll();

        void Add(T entity);

        void Remove(string id);
    }
}

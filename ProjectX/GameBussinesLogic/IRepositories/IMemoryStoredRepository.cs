using GameBussinesLogic.Models;
using System.Collections.Generic;

namespace GameBussinesLogic.Repositories
{
    public interface IMemoryStoredRepository<T> where T : BaseModel
    {
        T Get(string id);

        IList<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Remove(string id);
    }
}

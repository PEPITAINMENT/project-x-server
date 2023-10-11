using GameBussinesLogic.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameBussinesLogic.Repositories
{
    public abstract class MemoryStoredRepository<T> : IMemoryStoredRepository<T> where T : BaseModel
    {
        protected ConcurrentDictionary<string, T> Entyties = new ConcurrentDictionary<string, T>();

        public T Get(string id) {
            if (!Entyties.ContainsKey(id)) {
                return null;
            }
            
            return Entyties[id];
        }

        public ICollection<T> GetAll()
        {
            return Entyties.Values;
        }

        public void Add(T entity) {
            if (!Entyties.ContainsKey(entity.Id))
            {
                Entyties.AddOrUpdate(entity.Id, entity, (key, value) => value);
            }
        }

        public void Remove(string id) {
            if (!Entyties.ContainsKey(id))
            {
                return;
            }

            Entyties.TryRemove(id, out var value);
            if(Entyties.Count== 0)
            {
                ClearMemory();
            }
        }

        private void ClearMemory() {
            Entyties = new ConcurrentDictionary<string, T>();
        }
    }
}

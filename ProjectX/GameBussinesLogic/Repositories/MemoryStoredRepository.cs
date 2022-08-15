using GameBussinesLogic.Models;
using System.Collections.Generic;
using System.Linq;

namespace GameBussinesLogic.Repositories
{
    public abstract class MemoryStoredRepository<T> : IMemoryStoredRepository<T> where T : BaseModel
    {
        protected List<T> Entyties = new List<T>();

        public T Get(string id) {
            var currentEntityState = Entyties.FirstOrDefault(x => x.Id == id);

            return currentEntityState;
        }

        public IList<T> GetAll()
        {
            return Entyties;
        }

        public void Add(T entity) {
            Entyties.Add(entity);
        }

        public void Update(T entity) {
            var currentEntityState = Get(entity.Id);
            if (currentEntityState != null) {
                UpdateFields(currentEntityState, entity);
            }
        }

        public void Remove(string id) {
            var entity = Get(id);
            if (entity != null) {
                Entyties.Remove(entity);
                if (Entyties.Count == 0) {
                    ClearMemory();
                }
            }
        }

        private void ClearMemory() {
            Entyties = new List<T>();
        }

        protected abstract void UpdateFields(T currentEntityState, T newEntityState);
    }
}

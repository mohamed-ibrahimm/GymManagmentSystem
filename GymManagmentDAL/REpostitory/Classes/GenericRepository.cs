using GymManagmentDAL.Data.Context;
using GymManagmentDAL.REpostitory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentDAL.REpostitory.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly GymDBContext _dBContext;

        public GenericRepository(GymDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public void Add(TEntity entity) => _dBContext.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => _dBContext.Set<TEntity>().Remove(entity);

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition is null)
                return _dBContext.Set<TEntity>().AsNoTracking().ToList();

            return _dBContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
        }

        public TEntity? GetById(int id) => _dBContext.Set<TEntity>().Find(id);

        public void Update(TEntity entity) => _dBContext.Set<TEntity>().Update(entity);
    }
}
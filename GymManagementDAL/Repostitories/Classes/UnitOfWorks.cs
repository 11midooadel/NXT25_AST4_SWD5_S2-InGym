using GymManagementDAL.Data.DbContexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostitories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostitories.Classes
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly GymDbContext _dbContext;

        public UnitOfWorks(GymDbContext dbContext, ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        private readonly Dictionary<Type, object> _repositories = new();

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            //return new GenericRepository<TEntity>(_dbContext);
            var EntityType = typeof(TEntity);
            if (_repositories.ContainsKey(EntityType))
                return (IGenericRepository<TEntity>)_repositories[EntityType];

            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using System.ComponentModel.DataAnnotations;

namespace SWD392.Manim.Repositories.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Swd392Context _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool disposed = false;

        public UnitOfWork(Swd392Context dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }

            var repositoryInstance = new GenericRepository<T>(_dbContext);
            _repositories.Add(typeof(T), repositoryInstance);
            return repositoryInstance;
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _dbContext.Database.RollbackTransaction();
        }
    }
}

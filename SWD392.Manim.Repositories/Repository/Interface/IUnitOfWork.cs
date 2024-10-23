using Microsoft.EntityFrameworkCore;

namespace SWD392.Manim.Repositories.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}

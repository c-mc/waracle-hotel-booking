using System.Data;
using System.Linq.Expressions;

namespace HotelBookings.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<T?> GetByIdAsync(int id);

        Task<T?> GetWithSpecAsync(ISpecification<T> specification);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> specification);

        IQueryable<T> ExpressionToQuery(Expression<Func<T, bool>> expression);

        Task<int> CountAsync(ISpecification<T> specification);

        Task<int> AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        int DeleteAll();

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync(IsolationLevel isolationLevel);

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}

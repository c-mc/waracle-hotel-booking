using HotelBookings.Domain.Interfaces;
using HotelBookings.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace HotelBookings.Infrastructure.Repositories
{
    public class GenericRepository<T>(HotelBookingContext dbContext) : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> DbSet = dbContext.Set<T>();

        private async Task<T?> GetByIdBaseAsync(object id) {
            return await DbSet.FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await GetByIdBaseAsync(id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await GetByIdBaseAsync(id);
        }

        public async Task<T?> GetWithSpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public IQueryable<T> ExpressionToQuery(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
        }

        public async Task<int> AddAsync(T entity)
        {
            dbContext.Add(entity);
            return await SaveChangesAsync();
        }

        public void Update(T entity)
        {
            dbContext.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public int DeleteAll()
        {
            return DbSet.ExecuteDelete<T>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            await dbContext.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task CommitTransactionAsync()
        {
            await dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await dbContext.Database.RollbackTransactionAsync();
        }
    }
}

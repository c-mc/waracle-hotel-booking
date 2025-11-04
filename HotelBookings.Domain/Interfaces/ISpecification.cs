using System.Linq.Expressions;

namespace HotelBookings.Domain.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>>? Includes { get; }

        Expression<Func<T, object>>? OrderBy { get; }

        Expression<Func<T, object>>? OrderByDescending { get; }

        int? Take { get; }

        int? Skip { get; }

        void ApplyPagging(int take, int skip);
    }
}

using HotelBookings.Domain.Interfaces;
using System.Linq.Expressions;

namespace HotelBookings.Domain.Specifications
{
    public class BaseSpecification<T>(Expression<Func<T, bool>> criteria) : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; } = criteria;

        public List<Expression<Func<T, object>>> Includes { get; } = [];

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public int? Take { get; private set; }

        public int? Skip { get; private set; }

        public bool PagingEnabled { get; private set; }

        public void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public void AddOrderBy(Expression<Func<T, object>> OrderByexpression)
        {
            OrderBy = OrderByexpression;
        }

        public void AddOrderByDecending(Expression<Func<T, object>> OrderByDecending)
        {
            OrderByDescending = OrderByDecending;
        }

        public void ApplyPagging(int take, int skip)
        {
            Take = take;
            Skip = skip;
            PagingEnabled = true;
        }
    }
}

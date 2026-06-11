using System.Linq.Expressions;

namespace TicketingSystem.Repository.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; protected set; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();

        protected void AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }
    }
}
using TicketingSystem.Repository.Specifications;

namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface IGenericRepository<T> where T : class 
    {
        IQueryable<T> Query(ISpecification<T> spec);

        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> ListAsync(ISpecification<T> spec);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
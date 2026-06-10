namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface IGenericRepository<T> where T : class 
    {
        IQueryable<T> Query();

        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
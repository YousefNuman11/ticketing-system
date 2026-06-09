namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface IGenericRepository<T> where T : class 
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
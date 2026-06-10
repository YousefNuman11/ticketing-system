using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Repositories.Abstraction;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TicketingSystemDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(TicketingSystemDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IQueryable<T> Query()
        => _dbSet.AsQueryable();

    public async Task<List<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);
}
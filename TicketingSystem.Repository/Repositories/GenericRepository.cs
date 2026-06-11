using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Repositories.Abstraction;
using TicketingSystem.Repository.Specifications;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TicketingSystemDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(TicketingSystemDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IQueryable<T> Query(ISpecification<T> spec)
    {
        IQueryable<T> query = _context.Set<T>();

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        foreach (var include in spec.Includes)
            query = query.Include(include);

        return query;
    }

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

    public async Task<List<T>> ListAsync(ISpecification<T> spec)
    {
        IQueryable<T> query = _context.Set<T>();

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        foreach (var include in spec.Includes)
            query = query.Include(include);

        return await query.ToListAsync();
    }
}
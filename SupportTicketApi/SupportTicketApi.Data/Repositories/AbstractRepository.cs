using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Data.Contexts;
using System.Linq.Expressions;

namespace SupportTicketApi.Data.Repositories;

public abstract class AbstractRepository<T>(CrmContext context) where T: class
{
    protected readonly CrmContext _context = context;

    protected DbSet<T> Entities => RepositoryEntities();

    protected abstract DbSet<T> RepositoryEntities();

    public virtual IEnumerable<T> GetAll(params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return [.. query];
    }

    public virtual T? GetById(int id, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
    }

    public virtual IEnumerable<T> GetByIds(IEnumerable<int> ids, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.Where(e => ids.Contains(EF.Property<int>(e, "Id")));
    }

    public virtual void Insert(T entity) => Entities.Add(entity);

    public virtual void InsertRange(IEnumerable<T> entities) => Entities.AddRange(entities);

    public virtual void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

    public virtual bool Delete(int id)
    {
        var entity = Entities.Find(id);

        if (entity == null)
            return false;

        Entities.Remove(entity);
        return true;
    }

    public virtual void Save() => _context.SaveChanges();

    public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public virtual async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.Where(e => ids.Contains(EF.Property<int>(e, "Id"))).ToListAsync();
    }

    public virtual async Task InsertAsync(T entity) => await Entities.AddAsync(entity);

    public virtual async Task InsertRangeAsync(IEnumerable<T> entities) => await Entities.AddRangeAsync(entities);

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await Entities.FindAsync(id);

        if (entity == null)
            return false;

        Entities.Remove(entity);
        return true;
    }

    public virtual async Task UpdateAsync(T entity) => _context.Entry(entity).State = EntityState.Modified;

    public virtual async Task SaveAsync() => await _context.SaveChangesAsync();
}

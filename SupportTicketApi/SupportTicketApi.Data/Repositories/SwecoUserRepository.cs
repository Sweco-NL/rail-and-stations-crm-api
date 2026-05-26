using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;
using System.Linq.Expressions;

namespace SupportTicketApi.Data.Repositories;

public class SwecoUserRepository(CrmContext context) : AbstractRepository<SwecoUser>(context)
{
    protected override DbSet<SwecoUser> RepositoryEntities() => _context.SwecoUsers;

    public SwecoUser? GetBySwecoId(string swecoId, params Expression<Func<SwecoUser, object>>[] includes)
    {
        IQueryable<SwecoUser> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.FirstOrDefault(e => EF.Property<string>(e, "SwecoId") == swecoId);
    }

    public async Task<SwecoUser?> GetBySwecoIdAsync(string swecoId, params Expression<Func<SwecoUser, object>>[] includes)
    {
        IQueryable<SwecoUser> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "SwecoId") == swecoId);
    }

    public IEnumerable<SwecoUser> GetBySwecoIds(IEnumerable<string> swecoIds, params Expression<Func<SwecoUser, object>>[] includes)
    {
        IQueryable<SwecoUser> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.Where(e => swecoIds.Contains(EF.Property<string>(e, "SwecoId")));
    }

    public async Task<IEnumerable<SwecoUser>> GetBySwecoIdsAsync(IEnumerable<string> swecoIds, params Expression<Func<SwecoUser, object>>[] includes)
    {
        IQueryable<SwecoUser> query = Entities;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.Where(e => swecoIds.Contains(EF.Property<string>(e, "SwecoId"))).ToListAsync();
    }
}

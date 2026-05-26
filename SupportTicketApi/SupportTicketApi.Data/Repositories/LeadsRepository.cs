using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class LeadRepository(CrmContext context) : AbstractRepository<Lead>(context)
{
    protected override DbSet<Lead> RepositoryEntities() => _context.Leads;

    public void InsertWithDisciplines(Lead lead, IEnumerable<int> disciplineIds)
    {
        lead.Disciplines = [.. _context.Disciplines.Where(d => disciplineIds.Contains(d.Id))];
        Insert(lead);
    }

    public async Task InsertWithDisciplinesAsync(Lead lead, IEnumerable<int> disciplineIds)
    {
        lead.Disciplines = [.. await _context.Disciplines.Where(d => disciplineIds.Contains(d.Id)).ToListAsync()];
        await InsertAsync(lead);
    }
}

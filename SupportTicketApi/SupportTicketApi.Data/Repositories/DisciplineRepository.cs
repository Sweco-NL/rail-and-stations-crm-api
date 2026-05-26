using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class DisciplineRepository(CrmContext context) : AbstractRepository<Discipline>(context)
{
    protected override DbSet<Discipline> RepositoryEntities() => _context.Disciplines;
}

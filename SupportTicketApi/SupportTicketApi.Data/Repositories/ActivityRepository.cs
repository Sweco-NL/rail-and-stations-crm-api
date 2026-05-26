using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class ActivityRepository(CrmContext context) : AbstractRepository<Activity>(context)
{
    protected override DbSet<Activity> RepositoryEntities() => _context.Activities;
}

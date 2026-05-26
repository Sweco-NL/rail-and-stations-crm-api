using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class ActivityTypeRepository(CrmContext context) : AbstractRepository<ActivityType>(context)
{
    protected override DbSet<ActivityType> RepositoryEntities() => _context.ActivityTypes;
}

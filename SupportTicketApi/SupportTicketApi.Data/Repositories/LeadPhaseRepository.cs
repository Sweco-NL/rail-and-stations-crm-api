using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class LeadPhaseRepository(CrmContext context) : AbstractRepository<LeadPhase>(context)
{
    protected override DbSet<LeadPhase> RepositoryEntities() => _context.LeadPhases;
}

using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class CompanyTypeRepository(CrmContext context) : AbstractRepository<CompanyType>(context)
{
    protected override DbSet<CompanyType> RepositoryEntities() => _context.CompanyTypes;
}

using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class CompanyRepository(CrmContext context) : AbstractRepository<Company>(context)
{
    protected override DbSet<Company> RepositoryEntities() => _context.Companies;
}

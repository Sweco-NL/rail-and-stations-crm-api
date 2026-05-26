using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class ContactRepository(CrmContext context) : AbstractRepository<Contact>(context)
{
    protected override DbSet<Contact> RepositoryEntities() => _context.Contacts;
}

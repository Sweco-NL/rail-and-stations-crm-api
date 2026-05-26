using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Core.Models.Enums;
using SupportTicketApi.Data.Contexts;

namespace SupportTicketApi.Data.Repositories;

public class SwecoUserRoleRepository(CrmContext context) : AbstractRepository<SwecoUserRole>(context)
{
    protected override DbSet<SwecoUserRole> RepositoryEntities() => _context.SwecoUserRoles;

    public SwecoUserRole? GetSwecoUserRole(Role role) => Entities.FirstOrDefault(e => EF.Property<Role>(e, "Role") == role);

    public Task<SwecoUserRole?> GetSwecoUserRoleAsync(Role role) => Entities.FirstOrDefaultAsync(e => EF.Property<Role>(e, "Role") == role);
}

using SupportTicketApi.Core.Models.Enums;

namespace SupportTicketApi.Core.Models;

public class SwecoUserRole
{
    public int Id { get; set; }
    public Role Role { get; set; }

    public ICollection<SwecoUser> SwecoUsers { get; set; }
}

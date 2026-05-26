namespace SupportTicketApi.Core.Models;

public sealed class SwecoUser
{
    public int Id { get; set; }

    public string SwecoId { get; set; } = string.Empty;

    public bool Active { get; set; }

    public ICollection<SwecoUserRole> SwecoUserRoles { get; set; } = [];
}

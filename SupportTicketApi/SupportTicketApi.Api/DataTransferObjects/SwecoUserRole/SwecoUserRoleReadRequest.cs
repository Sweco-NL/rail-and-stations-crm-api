using SupportTicketApi.Core.Models.Enums;

namespace SupportTicketApi.Api.DataTransferObjects.SwecoUser;

public sealed class SwecoUserRoleReadRequest
{
    public int Id { get; set; }
    public Role Role { get; set; }
    public string RoleString => Role.ToString();
}

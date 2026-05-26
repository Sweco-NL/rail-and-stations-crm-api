using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.SwecoUser;

public sealed class SwecoUserReadRequest
{
    public int Id { get; set; }

    [Required]
    public string SwecoId { get; set; }

    public List<SwecoUserRoleReadRequest> SwecoUserRoles { get; set; } = [];

    public bool Active { get; set; } = false;
}

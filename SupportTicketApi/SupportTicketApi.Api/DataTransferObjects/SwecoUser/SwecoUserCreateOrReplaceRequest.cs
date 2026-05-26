using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.SwecoUser;

public sealed class SwecoUserCreateOrReplaceRequest
{
    [Required]
    public string SwecoId { get; set; }

    public bool Active { get; set; } = false;
}

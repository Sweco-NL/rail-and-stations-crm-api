using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.ActivityType;

public sealed class ActivityTypeCreateOrReplaceRequest
{
    [Required]
    public string Value { get; set; }
}

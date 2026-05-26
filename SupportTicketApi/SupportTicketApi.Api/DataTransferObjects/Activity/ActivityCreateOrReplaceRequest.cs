using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.Activity;

public sealed class ActivityCreateOrReplaceRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public int ActivityTypeId { get; set; }

    public int? ContactId { get; set; }

    public int? LeadId { get; set; }
    public string? SwecoContact { get; set; }
    public string? SwecoProject { get; set; }
    public DateTime? Date { get; set; }
    public string? Notes { get; set; }
}

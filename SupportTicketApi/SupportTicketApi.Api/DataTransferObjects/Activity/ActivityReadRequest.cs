using SupportTicketApi.Api.DataTransferObjects.Company;
using SupportTicketApi.Api.DataTransferObjects.ActivityType;
using SupportTicketApi.Api.DataTransferObjects.Contact;
using SupportTicketApi.Api.DataTransferObjects.Lead;
using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.Activity;

public sealed class ActivityReadRequest
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int CompanyId { get; set; }
    public CompanyReadRequest Company { get; set; }

    [Required]
    public int ActivityTypeId { get; set; }
    public ActivityTypeReadRequest ActivityType { get; set; }

    public int? ContactId { get; set; }
    public ContactReadRequest Contact { get; set; }

    public int? LeadId { get; set; }
    public LeadReadRequest Lead { get; set; }

    public string? SwecoContact { get; set; }
    public string? SwecoProject { get; set; }
    public DateTime? Date { get; set; }
    public string? Notes { get; set; }
}

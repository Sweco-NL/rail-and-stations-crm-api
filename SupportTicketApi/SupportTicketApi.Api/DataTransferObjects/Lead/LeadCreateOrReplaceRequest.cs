using SupportTicketApi.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.Lead;

public sealed class LeadCreateOrReplaceRequest
{
    [Required]
    public string Subject { get; set; }

    [Required]
    public int CompanyId { get; set; }

    public int? ContactId { get; set; }

    public ICollection<int>? DisciplinesIds { get; set; }

    public int? LeadPhaseId { get; set; }

    public string? ContactRole { get; set; }
    public string LeadSignalerSweco { get; set; }
    public string SwecoContact { get; set; }
    public int? ProjectSize { get; set; }
    public DateTime? Date { get; set; }

    public string? Context { get; set; }
    public string? CustomerConcerns { get; set; }
    public string? Opportunities { get; set; }
    public string? Summary { get; set; }
}

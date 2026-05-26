namespace SupportTicketApi.Api.DataTransferObjects.Lead;

public sealed class LeadUpdateRequest
{
    public string? Subject { get; set; }

    public int? ContactId { get; set; }

    public int? CompanyId { get; set; }

    public ICollection<int>? DisciplinesIds { get; set; }

    public int? LeadPhaseId { get; set; }

    public string? ContactRole { get; set; } = string.Empty;
    public string LeadSignalerSweco { get; set; } = string.Empty;
    public string SwecoContact { get; set; } = string.Empty;
    public int? ProjectSize { get; set; }
    public DateTime? Date { get; set; }

    public string? Context { get; set; }
    public string? CustomerConcerns { get; set; }
    public string? Opportunities { get; set; }
    public string? Summary { get; set; }
}

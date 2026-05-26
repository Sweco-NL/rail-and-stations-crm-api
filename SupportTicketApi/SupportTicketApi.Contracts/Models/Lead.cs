namespace SupportTicketApi.Core.Models;

public sealed class Lead
{
    public int Id { get; set; }

    public string Subject { get; set; } = string.Empty;

    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public int? ContactId { get; set; }
    public Contact? Contact { get; set; }

    public int? LeadPhaseId { get; set; }
    public LeadPhase? LeadPhase { get; set; }

    public ICollection<Discipline> Disciplines { get; set; } = [];

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

using SupportTicketApi.Api.DataTransferObjects.Company;
using SupportTicketApi.Api.DataTransferObjects.Contact;
using SupportTicketApi.Api.DataTransferObjects.Discipline;
using SupportTicketApi.Api.DataTransferObjects.LeadPhase;

namespace SupportTicketApi.Api.DataTransferObjects.Lead;

public sealed class LeadReadRequest
{
    public int Id { get; set; }

    public string Subject { get; set; }

    public int ContactId { get; set; }

    public ContactReadRequest Contact { get; set; }

    public int CompanyId { get; set; }

    public CompanyReadRequest Company { get; set; }

    public ICollection<int>? DisciplinesIds { get; set; }

    public ICollection<DisciplineReadRequest> Disciplines { get; set; }

    public int? LeadPhaseId { get; set; }
    public LeadPhaseReadRequest? LeadPhase { get; set; }

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

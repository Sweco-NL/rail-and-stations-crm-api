namespace SupportTicketApi.Core.Models;

public sealed class LeadPhase
{
    public int Id { get; set; }

    public string Value { get; set; }

    public ICollection<Lead> Leads { get; set; } = [];
}

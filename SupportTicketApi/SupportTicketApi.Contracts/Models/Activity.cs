namespace SupportTicketApi.Core.Models;

public sealed class Activity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public int ActivityTypeId { get; set; }
    public ActivityType ActivityType { get; set; }

    public int? ContactId { get; set; }
    public Contact? Contact { get; set; }

    public int? LeadId { get; set; }
    public Lead? Lead { get; set; }

    // TODO: rewrite to use SwecoUser
    public string? SwecoContact { get; set; }
    public string? SwecoProject { get; set; }
    public DateTime? Date { get; set; }
    public string? Notes { get; set; }
}

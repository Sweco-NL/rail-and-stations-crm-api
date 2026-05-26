namespace SupportTicketApi.Api.DataTransferObjects.Activity;

public sealed class ActivityUpdateRequest
{
    public string? Name { get; set; }

    public int? CompanyId { get; set; }
    public int? ActivityTypeId { get; set; }

    public int? ContactId { get; set; }

    public int? LeadId { get; set; }

    public string? SwecoContact { get; set; }
    public string? SwecoProject { get; set; }
    public DateTime? Date { get; set; }
    public string? Notes { get; set; }
}

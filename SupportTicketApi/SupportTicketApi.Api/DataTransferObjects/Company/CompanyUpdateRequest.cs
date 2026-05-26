namespace SupportTicketApi.Api.DataTransferObjects.Company;

public sealed class CompanyUpdateRequest
{
    public string? Name { get; set; } = string.Empty;
    public long? AnnualTurnover { get; set; }
    public ICollection<string> AccountManagers { get; set; } = [];
    public string? BoardName { get; set; }
    public string? Strengths { get; set; }
    public string? Weaknesses { get; set; }
    public string? Opportunities { get; set; }
    public string? Threats { get; set; }
    public string? Mission { get; set; }
    public string? Vision { get; set; }
    public string? StrategicMovement { get; set; }
    public string? BoardStrategy { get; set; }
    public string? AccountGoal { get; set; }
    public string? CompanyValues { get; set; }
}

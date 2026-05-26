namespace SupportTicketApi.Core.Models;

public sealed class CompanyType
{
    public int Id { get; set; }
    public string Value { get; set; }

    public ICollection<Company> Companies { get; set; }
}

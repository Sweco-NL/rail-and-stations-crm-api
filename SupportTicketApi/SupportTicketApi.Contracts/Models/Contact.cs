namespace SupportTicketApi.Core.Models;

public sealed class Contact
{
    public int Id { get; set; } 

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public int? DirectManagerId { get; set; }
    public Contact? DirectManager { get; set; }

    // TODO: rewrite to UserSweco
    public List<string> SwecoContacts { get; set; } = [];
    public string? JobTitle { get; set; }


    public string? Notes { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

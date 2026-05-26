using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.Contact;

public sealed class ContactCreateOrReplaceRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }

    public int? CompanyId { get; set; }
    public int? DirectManagerId { get; set; }

    public List<string>? SwecoContacts { get; set; }
    public string? JobTitle { get; set; }
    public string? Notes { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
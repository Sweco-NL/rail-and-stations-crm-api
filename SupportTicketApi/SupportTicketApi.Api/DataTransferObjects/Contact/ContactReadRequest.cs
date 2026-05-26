using SupportTicketApi.Api.DataTransferObjects.Company;
using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.Contact;

public sealed class ContactReadRequest
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }

    [Required]
    public int CompanyId { get; set; }
    public CompanyReadRequest Company { get; set; }

    public int? DirectManagerId { get; set; }
    public ContactReadRequest? DirectManager { get; set; }

    // TODO: rewrite to UserSweco
    public List<string> SwecoContacts { get; set; }
    public string? JobTitle { get; set; }
    public string? Notes { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
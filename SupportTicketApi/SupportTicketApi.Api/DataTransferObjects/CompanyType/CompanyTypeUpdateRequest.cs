using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects;

public sealed class CompanyTypeUpdateRequest
{
    [Required]
    public string Value { get; set; }
}

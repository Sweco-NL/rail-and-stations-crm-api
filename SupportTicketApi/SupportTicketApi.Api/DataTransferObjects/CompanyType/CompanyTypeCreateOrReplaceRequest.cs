using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects;

public sealed class CompanyTypeCreateOrReplaceRequest
{
    [Required]
    public string Value { get; set; }
}

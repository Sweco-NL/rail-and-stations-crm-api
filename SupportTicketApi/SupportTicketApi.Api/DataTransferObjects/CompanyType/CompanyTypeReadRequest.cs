using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects;

public sealed class CompanyTypeReadRequest
{
    public int Id { get; set; }
    public string Value { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.Discipline;

public sealed class DisciplineCreateOrReplaceRequest
{
    [Required]
    public string Value { get; set; }
}

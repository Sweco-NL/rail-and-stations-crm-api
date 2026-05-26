using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.Discipline;

public sealed class DisciplineReadRequest
{
    public int Id { get; set; }

    [Required]
    public string Value { get; set; }
}

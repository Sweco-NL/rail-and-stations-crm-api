using System.ComponentModel.DataAnnotations;

namespace SupportTicketApi.Api.DataTransferObjects.LeadPhase;

public class LeadPhaseReadRequest
{
    public int Id { get; set; }

    [Required]
    public string Value { get; set; }
}


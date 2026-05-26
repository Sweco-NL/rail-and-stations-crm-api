using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SupportTicketApi.Api.DataTransferObjects.LeadPhase;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class LeadPhasesController(LeadPhaseRepository leadPhaseRepository, IMapper mapper) : BaseApiController
{
    private readonly LeadPhaseRepository _leadPhaseRepository = leadPhaseRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/LeadPhases
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeadPhaseReadRequest>>> GetLeadPhases()
    {
        var leadPhases = await _leadPhaseRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<LeadPhase>, IEnumerable<LeadPhaseReadRequest>>(leadPhases));
    }

    // GET: api/LeadPhases/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LeadPhaseReadRequest>> GetLeadPhase(int id)
    {
        var leadPhases = await _leadPhaseRepository.GetByIdAsync(id);

        if (leadPhases == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<LeadPhaseReadRequest>(leadPhases));
    }
}

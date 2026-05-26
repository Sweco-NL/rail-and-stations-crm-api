using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects.Lead;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class LeadsController(LeadRepository leadRepository, IMapper mapper) : BaseApiController
{
    private readonly LeadRepository _leadRepository = leadRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/Leads
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeadReadRequest>>> GetLeads()
    {
        var leads = await _leadRepository.GetAllAsync(
            l => l.Company,
            l => l.Contact,
            l => l.Disciplines,
            l => l.LeadPhase);

        return Ok(_mapper.Map<IEnumerable<Lead>, IEnumerable<LeadReadRequest>>(leads));
    }

    // GET: api/Leads/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LeadReadRequest>> GetLead(int id)
    {
        var leads = await _leadRepository.GetByIdAsync(id, l => l.LeadPhase, l => l.Company, l => l.Contact, l => l.Disciplines);

        if (leads == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<LeadReadRequest>(leads));
    }

    // PUT: api/Leads/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLead(int id, LeadCreateOrReplaceRequest leadUpdateRequest)
    {
        var lead = await _leadRepository.GetByIdAsync(id);

        if (lead == null)
        {
            return NotFound();
        }

        _mapper.Map(leadUpdateRequest, lead);

        await _leadRepository.UpdateAsync(lead);

        try
        {
            await _leadRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_leadRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // PATCH: api/Leads/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchLead(
        int id,
        [FromBody] JsonPatchDocument<LeadCreateOrReplaceRequest> patchRequest)
    {
        if (patchRequest == null || patchRequest.Operations.Count == 0)
        {
            return BadRequest("No patch operations provided");
        }

        var lead = await _leadRepository.GetByIdAsync(id);
        if (lead == null)
        {
            return NotFound();
        }

        var leadCreateOrReplaceRequest = _mapper.Map<LeadCreateOrReplaceRequest>(lead);

        // by filling a CompanyCreateOrReplaceRequest with the patch request, we make sure an API caller cannot
        // circumvent business logic that applies to the company by using a badly formatted request
        patchRequest.ApplyTo(leadCreateOrReplaceRequest, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the patched DTO back to the entity
        _mapper.Map(leadCreateOrReplaceRequest, lead);

        await _leadRepository.UpdateAsync(lead);

        try
        {
            await _leadRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_leadRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Leads
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<LeadReadRequest>> PostLead(LeadCreateOrReplaceRequest leadCreateRequest)
    {
        Lead lead = _mapper.Map<Lead>(leadCreateRequest);

        lead.LeadPhase = null;

        await _leadRepository.InsertWithDisciplinesAsync(lead, leadCreateRequest.DisciplinesIds ?? []);
        await _leadRepository.SaveAsync();

        var completeLead = await _leadRepository.GetByIdAsync(
                lead.Id,
                c => c.Company,
                c => c.Disciplines,
                c => c.Contact,
                c => c.LeadPhase
            );

        if (completeLead == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<LeadReadRequest>(completeLead);

        return CreatedAtAction("GetLead", new { id = result.Id }, result);
    }

    // DELETE: api/Leads/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLead(int id)
    {
        var success = _leadRepository.Delete(id);

        if (!success)
        {
            return NotFound();
        }

        await _leadRepository.SaveAsync();

        return NoContent();
    }
}

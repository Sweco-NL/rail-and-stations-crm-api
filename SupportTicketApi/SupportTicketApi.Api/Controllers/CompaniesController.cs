using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects.Company;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class CompaniesController(CompanyRepository companyRepository, IMapper mapper) : BaseApiController
{
    private readonly CompanyRepository _companyRepository = companyRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/Companies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyReadRequest>>> GetCompanies()
    {
        var companies = await _companyRepository.GetAllAsync(c => c.CompanyType);
        return Ok(_mapper.Map<IEnumerable<Company>, IEnumerable<CompanyReadRequest>>(companies));
    }

    // GET: api/Companies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyReadRequest>> GetCompany(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id, c => c.CompanyType);

        if (company == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CompanyReadRequest>(company));
    }

    // PUT: api/Companies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCompany(int id, CompanyCreateOrReplaceRequest companyUpdate)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        _mapper.Map(companyUpdate, company);

        await _companyRepository.UpdateAsync(company);

        try
        {
            await _companyRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_companyRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // PATCH: api/Companies/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCompany(
        int id, 
        [FromBody] JsonPatchDocument<CompanyCreateOrReplaceRequest> patchRequest)
    {
        if (patchRequest == null || patchRequest.Operations.Count == 0)
        {
            return BadRequest("No patch operations provided");
        }

        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        var companyCreateOrReplaceRequest = _mapper.Map<CompanyCreateOrReplaceRequest>(company);
        
        // by filling a CompanyCreateOrReplaceRequest with the patch request, we make sure an API caller cannot
        // circumvent business logic that applies to the company by using a badly formatted request
        patchRequest.ApplyTo(companyCreateOrReplaceRequest, ModelState);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the patched DTO back to the entity
        _mapper.Map(companyCreateOrReplaceRequest, company);

        await _companyRepository.UpdateAsync(company);

        try
        {
            await _companyRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_companyRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Companies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CompanyReadRequest>> PostCompany(CompanyCreateOrReplaceRequest companyCreateRequest)
    {
        Company company = _mapper.Map<Company>(companyCreateRequest);
        _companyRepository.Insert(company);
        await _companyRepository.SaveAsync();

        var completeCompany = await _companyRepository.GetByIdAsync(company.Id, (c) => c.CompanyType);

        if (completeCompany == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CompanyReadRequest>(completeCompany);

        return CreatedAtAction("GetCompany", new { id = result.Id }, result);
    }

    // DELETE: api/Companies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var success = _companyRepository.Delete(id);

        if (!success)
            return NotFound();

        await _companyRepository.SaveAsync();

        return NoContent();
    }
}

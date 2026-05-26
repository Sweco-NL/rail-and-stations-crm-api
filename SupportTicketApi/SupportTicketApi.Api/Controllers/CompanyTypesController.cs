using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;
using System.Diagnostics;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class CompanyTypesController(CompanyTypeRepository companyRepository, IMapper mapper) : BaseApiController
{
    private readonly CompanyTypeRepository _companyTypeRepository = companyRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/CompanyTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyTypeReadRequest>>> GetCompanyTypes()
    {
        var companytypes = await _companyTypeRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<CompanyType>, IEnumerable<CompanyTypeReadRequest>>(companytypes));
    }

    // GET: api/CompanyTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyTypeReadRequest>> GetCompanyType(int id)
    {
        var companytypes = await _companyTypeRepository.GetByIdAsync(id);

        if (companytypes == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CompanyTypeReadRequest>(companytypes));
    }

    // PUT: api/CompanyTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Policy = "IsAdmin")]
    public async Task<IActionResult> PutCompanyType(int id, CompanyTypeCreateOrReplaceRequest companyTypeUpdateRequest)
    {
        var companyType = await _companyTypeRepository.GetByIdAsync(id);

        if (companyType == null)
        {
            return NotFound();
        }

        _mapper.Map(companyTypeUpdateRequest, companyType);

        await _companyTypeRepository.UpdateAsync(companyType);

        try
        {
            await _companyTypeRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_companyTypeRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/CompanyTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Policy = "IsAdmin")]
    public async Task<ActionResult<CompanyTypeReadRequest>> PostCompanyType(CompanyTypeCreateOrReplaceRequest companyTypeCreateRequest)
    {
        CompanyType companyType = _mapper.Map<CompanyType>(companyTypeCreateRequest);
        _companyTypeRepository.Insert(companyType);

        try
        {
            await _companyTypeRepository.SaveAsync();
            var result = _mapper.Map<CompanyTypeReadRequest>(companyType);

            return CreatedAtAction("GetCompanyType", new { id = result.Id }, result);
        } catch (DbUpdateException e)
        {
            // TODO: turn this into structured logging!
            Debug.WriteLine(e.Message);
            return UnprocessableEntity("Could not add company type. Likely, the value is already in use.");
        } catch (Exception e)
        {
            // TODO: turn this into structured logging!
            Debug.WriteLine(e.Message);
            return Problem("Could not post company type for unknown reasons.");
        }
    }

    // DELETE: api/CompanyTypes/5
    [HttpDelete("{id}")]
    [Authorize(Policy = "IsAdmin")]
    public async Task<IActionResult> DeleteCompanyType(int id)
    {
        var success = _companyTypeRepository.Delete(id);

        if (!success)
            return NotFound();

        await _companyTypeRepository.SaveAsync();

        return NoContent();
    }
}

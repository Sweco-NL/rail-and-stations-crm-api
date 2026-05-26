using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects.Discipline;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class DisciplinesController(DisciplineRepository disciplineRepository, IMapper mapper) : BaseApiController
{
    private readonly DisciplineRepository _disciplineRepository = disciplineRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/Disciplines
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DisciplineReadRequest>>> GetDisciplines()
    {
        var disciplines = await _disciplineRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<Discipline>, IEnumerable<DisciplineReadRequest>>(disciplines));
    }

    // GET: api/Disciplines/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DisciplineReadRequest>> GetDiscipline(int id)
    {
        var disciplines = await _disciplineRepository.GetByIdAsync(id);

        if (disciplines == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<DisciplineReadRequest>(disciplines));
    }

    // PUT: api/Disciplines/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDiscipline(int id, DisciplineCreateOrReplaceRequest disciplineUpdateRequest)
    {
        var discipline = await _disciplineRepository.GetByIdAsync(id);

        if (discipline == null)
        {
            return NotFound();
        }

        _mapper.Map(disciplineUpdateRequest, discipline);

        await _disciplineRepository.UpdateAsync(discipline);

        try
        {
            await _disciplineRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_disciplineRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Disciplines
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DisciplineReadRequest>> PostDiscipline(DisciplineCreateOrReplaceRequest disciplineCreateRequest)
    {
        Discipline discipline = _mapper.Map<Discipline>(disciplineCreateRequest);
        _disciplineRepository.Insert(discipline);
        await _disciplineRepository.SaveAsync();
        var result = _mapper.Map<DisciplineReadRequest>(discipline);

        return CreatedAtAction("GetDiscipline", new { id = result.Id }, result);
    }

    // DELETE: api/Disciplines/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscipline(int id)
    {
        var success = _disciplineRepository.Delete(id);

        if (!success)
            return NotFound();

        await _disciplineRepository.SaveAsync();

        return NoContent();
    }
}

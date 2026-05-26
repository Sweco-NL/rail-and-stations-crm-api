using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects.ActivityType;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class ActivityTypesController(ActivityTypeRepository activityRepository, IMapper mapper) : BaseApiController
{
    private readonly ActivityTypeRepository _activityTypeRepository = activityRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/ActivityTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityTypeReadRequest>>> GetActivityTypes()
    {
        var activitytypes = await _activityTypeRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ActivityType>, IEnumerable<ActivityTypeReadRequest>>(activitytypes));
    }

    // GET: api/ActivityTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityTypeReadRequest>> GetActivityType(int id)
    {
        var activitytypes = await _activityTypeRepository.GetByIdAsync(id);

        if (activitytypes == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ActivityTypeReadRequest>(activitytypes));
    }

    // PUT: api/ActivityTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutActivityType(int id, ActivityTypeCreateOrReplaceRequest activityTypeUpdateRequest)
    {
        var activityType = await _activityTypeRepository.GetByIdAsync(id);

        if (activityType == null)
        {
            return NotFound();
        }

        _mapper.Map(activityTypeUpdateRequest, activityType);

        await _activityTypeRepository.UpdateAsync(activityType);

        try
        {
            await _activityTypeRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_activityTypeRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/ActivityTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ActivityTypeReadRequest>> PostActivityType(ActivityTypeCreateOrReplaceRequest activityTypeCreateRequest)
    {
        ActivityType activityType = _mapper.Map<ActivityType>(activityTypeCreateRequest);
        _activityTypeRepository.Insert(activityType);
        await _activityTypeRepository.SaveAsync();
        var result = _mapper.Map<ActivityTypeReadRequest>(activityType);

        return CreatedAtAction("GetActivityType", new { id = result.Id }, result);
    }

    // DELETE: api/ActivityTypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivityType(int id)
    {
        var success = _activityTypeRepository.Delete(id);

        if (!success)
            return NotFound();

        await _activityTypeRepository.SaveAsync();

        return NoContent();
    }
}

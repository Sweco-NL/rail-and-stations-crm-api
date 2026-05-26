using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects.Activity;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class ActivitiesController(ActivityRepository activityRepository, IMapper mapper) : BaseApiController
{
    private readonly ActivityRepository _activityRepository = activityRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/Activities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityReadRequest>>> GetActivities()
    {
        var activities = await _activityRepository.GetAllAsync(
            a => a.Lead,
            a => a.Company,
            a => a.ActivityType,
            a => a.Contact);
        return Ok(_mapper.Map<IEnumerable<Activity>, IEnumerable<ActivityReadRequest>>(activities));
    }

    // GET: api/Activities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityReadRequest>> GetActivity(int id)
    {
        var activities = await _activityRepository.GetByIdAsync(id,
            a => a.Lead,
            a => a.Company,
            a => a.ActivityType,
            a => a.Contact);

        if (activities == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ActivityReadRequest>(activities));
    }

    // PUT: api/Activities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutActivity(int id, ActivityCreateOrReplaceRequest activityUpdateRequest)
    {
        var activity = await _activityRepository.GetByIdAsync(id);

        if (activity == null)
        {
            return NotFound();
        }

        _mapper.Map(activityUpdateRequest, activity);

        await _activityRepository.UpdateAsync(activity);

        try
        {
            await _activityRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_activityRepository.GetById(id) == null)
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
        [FromBody] JsonPatchDocument<ActivityCreateOrReplaceRequest> patchRequest)
    {
        if (patchRequest == null || patchRequest.Operations.Count == 0)
        {
            return BadRequest("No patch operations provided");
        }

        var activity = await _activityRepository.GetByIdAsync(id);
        if (activity == null)
        {
            return NotFound();
        }

        var activityreateOrReplaceRequest = _mapper.Map<ActivityCreateOrReplaceRequest>(activity);

        // by filling a CompanyCreateOrReplaceRequest with the patch request, we make sure an API caller cannot
        // circumvent business logic that applies to the company by using a badly formatted request
        patchRequest.ApplyTo(activityreateOrReplaceRequest, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the patched DTO back to the entity
        _mapper.Map(activityreateOrReplaceRequest, activity);

        await _activityRepository.UpdateAsync(activity);

        try
        {
            await _activityRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_activityRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }


    // POST: api/Activities
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ActivityReadRequest>> PostActivity(ActivityCreateOrReplaceRequest activityCreateRequest)
    {
        Activity activity = _mapper.Map<Activity>(activityCreateRequest);
        _activityRepository.Insert(activity);
        await _activityRepository.SaveAsync();

        var completeActivity = await _activityRepository.GetByIdAsync(activity.Id,
            a => a.Lead,
            a => a.Company,
            a => a.ActivityType,
            a => a.Contact);

        if (completeActivity == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ActivityReadRequest>(completeActivity);

        return CreatedAtAction("GetActivity", new { id = result.Id }, result);
    }

    // DELETE: api/Activities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var success = _activityRepository.Delete(id);

        if (!success)
            return NotFound();

        await _activityRepository.SaveAsync();

        return NoContent();
    }
}

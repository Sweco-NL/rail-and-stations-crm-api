using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects.SwecoUser;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;
using SupportTicketApi.Core.Models.Enums;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class SwecoUsersController(SwecoUserRepository swecoUserRepository, SwecoUserRoleRepository swecoUserRoleRepository, IMapper mapper) : BaseApiController
{
    private readonly SwecoUserRepository _swecoUserRepository = swecoUserRepository;
    private readonly SwecoUserRoleRepository _swecoUserRoleRepository = swecoUserRoleRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/SwecoUsers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SwecoUserReadRequest>>> GetSwecoUsers()
    {
        var swecoUsers = await _swecoUserRepository.GetAllAsync(swecoUser => swecoUser.SwecoUserRoles);
        return Ok(_mapper.Map<IEnumerable<SwecoUser>, IEnumerable<SwecoUserReadRequest>>(swecoUsers));
    }

    // GET: api/SwecoUsers/me
    [HttpGet("me")]
    public async Task<ActionResult<IEnumerable<SwecoUserReadRequest>>> GetCurrentUser()
    {
        return Ok(_mapper.Map<SwecoUserReadRequest>(CurrentUser));
    }

    // GET: api/SwecoUsers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SwecoUserReadRequest>> GetSwecoUser(int id)
    {
        var swecoUser = await _swecoUserRepository.GetByIdAsync(id);

        if (swecoUser == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<SwecoUserReadRequest>(swecoUser));
    }

    // PATCH: api/SwecoUsers/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchSwecoUser(
        int id,
        [FromBody] JsonPatchDocument<SwecoUserCreateOrReplaceRequest> patchRequest)
    {
        if (patchRequest == null || patchRequest.Operations.Count == 0)
        {
            return BadRequest("No patch operations provided");
        }

        var swecoUser = await _swecoUserRepository.GetByIdAsync(id);
        if (swecoUser == null)
        {
            return NotFound();
        }

        var swecoUserCreateOrReplaceRequest = _mapper.Map<SwecoUserCreateOrReplaceRequest>(swecoUser);

        // by filling a CompanyCreateOrReplaceRequest with the patch request, we make sure an API caller cannot
        // circumvent business logic that applies to the company by using a badly formatted request
        patchRequest.ApplyTo(swecoUserCreateOrReplaceRequest, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the patched DTO back to the entity
        _mapper.Map(swecoUserCreateOrReplaceRequest, swecoUser);

        await _swecoUserRepository.UpdateAsync(swecoUser);

        try
        {
            await _swecoUserRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_swecoUserRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/SwecoUsers
    [HttpPost]
    public async Task<IActionResult> PostSwecoUser(SwecoUserCreateOrReplaceRequest swecoUserCreateOrReplaceRequest)
    {
        return BadRequest();
    }

    // POST: api/SwecoUsers/several
    [HttpPost("several")]
    public async Task<ActionResult<IEnumerable<SwecoUserReadRequest>>> PostSwecoUsers([FromBody] List<SwecoUserCreateOrReplaceRequest> swecoUserCreateOrReplaceSeveralRequest)
    {
        var presentSwecoUsers = await _swecoUserRepository.GetBySwecoIdsAsync(
            swecoUserCreateOrReplaceSeveralRequest.Select(user => user.SwecoId)
        );

        // only create users which do not exist yet in the database
        var swecoUsersToUpdate = swecoUserCreateOrReplaceSeveralRequest
            .Where(user => !presentSwecoUsers.Any(presentSwecoUser => presentSwecoUser.SwecoId == user.SwecoId));

        List<SwecoUser> swecoUsers = _mapper.Map<List<SwecoUser>>(swecoUsersToUpdate);

        if (swecoUsers == null || swecoUsers.Count == 0)
        {
            return Ok();
        }

        var userRole = _swecoUserRoleRepository.GetSwecoUserRole(Role.User);

        if (userRole == null)
        {
            // The "User" role should always be present in the database!
            return StatusCode(500);
        }

        foreach (var swecoUser in swecoUsers)
        {
            swecoUser.SwecoUserRoles = [userRole];
        }

        await _swecoUserRepository.InsertRangeAsync(swecoUsers);
        await _swecoUserRepository.SaveAsync();

        var allSwecoUsers = await _swecoUserRepository.GetByIdsAsync(swecoUsers.Select(swecoUser => swecoUser.Id));
        if (allSwecoUsers == null || !allSwecoUsers.Any())
        {
            return NotFound();
        }

        var results = _mapper.Map<IEnumerable<SwecoUser>, IEnumerable<SwecoUserReadRequest>>(allSwecoUsers);

        return CreatedAtAction("GetSwecoUsers", new { ids = results.Select(result => result.Id) }, results);
    }
}

using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Api.DataTransferObjects.Contact;
using SupportTicketApi.Core.Models;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Controllers;

[Route("api/[controller]")]
public class ContactsController(ContactRepository contactRepository, IMapper mapper) : BaseApiController
{
    private readonly ContactRepository _contactRepository = contactRepository;
    private readonly IMapper _mapper = mapper;

    // GET: api/Contacts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactReadRequest>>> GetContacts()
    {
        var contacts = await _contactRepository.GetAllAsync(c => c.Company, c => c.DirectManager);
        return Ok(_mapper.Map<IEnumerable<Contact>, IEnumerable<ContactReadRequest>>(contacts));
    }

    // GET: api/Contacts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactReadRequest>> GetContact(int id)
    {
        var contacts = await _contactRepository.GetByIdAsync(id, c => c.Company, c => c.DirectManager);

        if (contacts == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ContactReadRequest>(contacts));
    }

    // PUT: api/Contacts/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutContact(int id, ContactCreateOrReplaceRequest contactUpdateRequest)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        _mapper.Map(contactUpdateRequest, contact);

        await _contactRepository.UpdateAsync(contact);

        try
        {
            await _contactRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_contactRepository.GetById(id) == null)
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }

    // PATCH: api/Contacts/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchContact(
        int id,
        [FromBody] JsonPatchDocument<ContactCreateOrReplaceRequest> patchRequest)
    {
        if (patchRequest == null || patchRequest.Operations.Count == 0)
        {
            return BadRequest("No patch operations provided");
        }

        var contact = await _contactRepository.GetByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        var contactCreateOrReplaceRequest = _mapper.Map<ContactCreateOrReplaceRequest>(contact);

        // by filling a CompanyCreateOrReplaceRequest with the patch request, we make sure an API caller cannot
        // circumvent business logic that applies to the company by using a badly formatted request
        patchRequest.ApplyTo(contactCreateOrReplaceRequest, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the patched DTO back to the entity
        _mapper.Map(contactCreateOrReplaceRequest, contact);

        await _contactRepository.UpdateAsync(contact);

        try
        {
            await _contactRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_contactRepository.GetById(id) == null)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Contacts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ContactReadRequest>> PostContact(ContactCreateOrReplaceRequest contactCreateRequest)
    {
        Contact contact = _mapper.Map<Contact>(contactCreateRequest);
        _contactRepository.Insert(contact);
        await _contactRepository.SaveAsync();

        var completeContact = await _contactRepository.GetByIdAsync(contact.Id, c => c.Company, c => c.DirectManager);

        if (completeContact == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ContactReadRequest>(completeContact);

        return CreatedAtAction("GetContact", new { id = result.Id }, result);
    }

    // DELETE: api/Contacts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var success = _contactRepository.Delete(id);

        if (!success)
            return NotFound();

        await _contactRepository.SaveAsync();

        return NoContent();
    }
}

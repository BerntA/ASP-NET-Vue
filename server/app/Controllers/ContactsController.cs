using lib.Models;
using lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ContactService _contactService;

    public ContactsController(ContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
        var contacts = await _contactService.Contacts();
        return new OkObjectResult(contacts);
    }

    [HttpPut]
    public async Task<IActionResult> AddContact([FromBody] CreateContact data)
    {
        var contact = await _contactService.AddContact(data);
        return contact is null ? new BadRequestResult() : new OkObjectResult(contact);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateContact([FromBody] UpdateContact data)
    {
        var updatedContact = await _contactService.UpdateContact(data);
        return updatedContact is null ? new BadRequestResult() : new OkObjectResult(updatedContact);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteContact([FromQuery] Guid id)
    {
        var removed = await _contactService.RemoveContact(id);
        return removed ? new OkResult() : new BadRequestResult();
    }
}
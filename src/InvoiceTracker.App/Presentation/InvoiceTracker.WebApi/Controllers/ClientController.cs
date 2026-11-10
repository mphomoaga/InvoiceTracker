using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracker.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,InvoiceManager,InvoiceAgent")]
public sealed class ClientController(IPersonManager personManager) : ControllerBase
{
    [HttpGet("{personId:int}")]
    public async Task<ActionResult<Person>> GetClientById(
        int personId,
        CancellationToken cancellationToken)
    {
        var person = await personManager.GetClientById(personId, cancellationToken);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<Person>> AddNewClient(
        [FromBody] Person person,
        CancellationToken cancellationToken)
    {
        var createdPerson = await personManager.AddNewClient(person, cancellationToken);
        return CreatedAtAction(
            nameof(GetClientById),
            new { personId = createdPerson.PersonId },
            createdPerson);
    }

    [HttpGet("page")]
    public async Task<ActionResult<PagedResult<Person>>> GetPageClient(
        [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
        [FromQuery, Range(1, 100)] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var page = await personManager.GetPageClient(pageNumber, pageSize, searchTerm, cancellationToken);
        return Ok(page);
    }

    [HttpDelete("{personId:int}")]
    public async Task<IActionResult> DeactivateClient(
        int personId,
        CancellationToken cancellationToken)
    {
        var modifiedBy = User.FindFirst(ClaimTypes.Email)?.Value
            ?? User.FindFirst("email")?.Value
            ?? "web-ui";

        var deactivated = await personManager.DeactivateClient(
            personId,
            modifiedBy,
            cancellationToken);

        return deactivated ? NoContent() : NotFound();
    }
}

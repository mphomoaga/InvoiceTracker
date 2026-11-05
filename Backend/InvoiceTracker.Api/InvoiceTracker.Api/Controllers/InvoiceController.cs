using InvoiceTracker.Domain.Interfaces.Services;
using InvoiceTracker.Domain.Models.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracker.Api.Controllers;

[ApiController]
[Authorize(Roles = "User")]
[Route("api/[controller]")]
public sealed class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("{invoiceId:int}")]
    public async Task<ActionResult<InvoiceDto>> GetById(
        int invoiceId,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoiceService.GetByIdAsync(invoiceId, cancellationToken);
        return invoice is null ? NotFound() : Ok(invoice);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<InvoiceDto>>> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var invoices = await _invoiceService.GetPagedAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        return Ok(invoices);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Add(
        [FromBody] CreateInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var invoice = await _invoiceService.AddAsync(request, cancellationToken);
            return CreatedAtAction(
                nameof(GetById),
                new { invoiceId = invoice.InvoiceId },
                invoice);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpDelete("{invoiceId:int}")]
    public async Task<IActionResult> Remove (
        int invoiceId,
        [FromQuery] string? modifiedBy,
        CancellationToken cancellationToken)
    {
        var deleted = await _invoiceService.RemoveInvoice(
            invoiceId,
            string.IsNullOrWhiteSpace(modifiedBy)
                ? User.Identity?.Name ?? "system"
                : modifiedBy,
            cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}

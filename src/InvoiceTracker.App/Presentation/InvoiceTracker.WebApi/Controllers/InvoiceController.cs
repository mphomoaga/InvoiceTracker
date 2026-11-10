using Azure.Core;
using System.Security.Claims;
using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Application.Services;
using InvoiceTracker.Domain.Enums;
using InvoiceTracker.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,InvoiceManager,InvoiceAgent")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService invoiceService,
            ILogger<InvoiceController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<PagedResult<Invoice>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            var invoices = await _invoiceService.GetPaged(
                pageNumber,
                pageSize,
                searchTerm,
                cancellationToken);

            return Ok(invoices);
        }

        [HttpDelete("{invoiceId:int}")]
        public async Task<IActionResult> DeactivateInvoice(
            int invoiceId,
            CancellationToken cancellationToken = default)
        {
            var modifiedBy = User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value
                ?? "web-ui";

            var deactivated = await _invoiceService.RemoveInvoice(
                invoiceId,
                modifiedBy,
                cancellationToken);

            return deactivated ? NoContent() : NotFound();
        }

        [HttpPost("{invoiceId:int}/email")]
        public async Task<IActionResult> EmailInvoice(
            int invoiceId,
            [FromBody] EmailInvoiceRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var createdBy = User.FindFirst(ClaimTypes.Email)?.Value
                    ?? User.FindFirst("email")?.Value
                    ?? "web-ui";

                var sent = await _invoiceService.EmailInvoice(
                    invoiceId,
                    request.RecipientEmail,
                    createdBy,
                    cancellationToken);

                return sent ? Ok() : BadRequest(new { message = "Could not email invoice." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while emailing invoice {InvoiceId}.", invoiceId);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> Add(
        [FromBody] CreateInvoiceRequest request,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var invoice = await _invoiceService.AddNewInvoice(request, cancellationToken);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invoice creation errror");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("UpdateInvoiceStatus/{invoiceId:int}/{invoiceStatusId:int}")]
        public async Task<IActionResult> UpdateInvoiceStatus(
            [FromRoute] int invoiceId,
            [FromRoute] InvoiceStatusEnum invoiceStatusId,
            CancellationToken cancellationToken = default)
        {

            try
            {
                var result = await _invoiceService.UpdateInvoiceStatus(
                    invoiceId,
                    invoiceStatusId,
                    cancellationToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while updating the status for invoice {invoiceId}.");

                return BadRequest(new { message = ex.Message });
            }


        }

        [HttpGet("GetInvoice/{invoiceId:int}")]
        public async Task<IActionResult> GetInvoice(
            [FromRoute] int invoiceId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _invoiceService.GetById(
                    invoiceId,
                    cancellationToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while retrieving invoice {invoiceId}.");

                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

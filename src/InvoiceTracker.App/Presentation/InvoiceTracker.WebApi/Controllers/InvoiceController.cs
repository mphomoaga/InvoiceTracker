using Azure.Core;
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
            CancellationToken cancellationToken = default)
        {
            var invoices = await _invoiceService.GetPaged(
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(invoices);
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

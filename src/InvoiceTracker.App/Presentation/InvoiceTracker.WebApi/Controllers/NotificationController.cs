using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracker.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,InvoiceManager,InvoiceAgent")]
public sealed class NotificationController(INotificationService notificationService, ILogger<NotificationController> logger) : ControllerBase
{
    [HttpGet("page")]
    public async Task<ActionResult<PagedResult<EmailAuditSummaryDto>>> GetPage(
        [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
        [FromQuery, Range(1, 100)] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var result = await notificationService.GetPage(pageNumber, pageSize, searchTerm, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{emailAuditId:int}")]
    public async Task<ActionResult<EmailAuditDetailsDto>> GetById(
        int emailAuditId,
        CancellationToken cancellationToken = default)
    {
        var email = await notificationService.GetById(emailAuditId, cancellationToken);
        return email is null ? NotFound() : Ok(email);
    }

    [HttpPost("{emailAuditId:int}/resend")]
    public async Task<IActionResult> Resend(
        int emailAuditId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var createdBy = User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value
                ?? "web-ui";
            var resent = await notificationService.Resend(emailAuditId, createdBy, cancellationToken);
            return resent ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while resending email audit {EmailAuditId}.", emailAuditId);
            return BadRequest(new { message = ex.Message });
        }
    }
}

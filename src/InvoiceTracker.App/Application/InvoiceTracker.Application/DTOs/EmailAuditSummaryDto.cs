namespace InvoiceTracker.Application.DTOs;

public sealed record EmailAuditSummaryDto(
    int EmailAuditId,
    string To,
    string? CC,
    string Subject,
    string ItemType,
    int ItemTypeRef,
    bool HasAttachments,
    bool IsSent,
    DateTime SentDate);

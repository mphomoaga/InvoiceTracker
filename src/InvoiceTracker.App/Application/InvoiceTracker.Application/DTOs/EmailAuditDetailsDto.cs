namespace InvoiceTracker.Application.DTOs;

public sealed record EmailAuditDetailsDto(
    int EmailAuditId,
    string To,
    string? CC,
    string Subject,
    string Body,
    string ItemType,
    int ItemTypeRef,
    bool HasAttachments,
    bool IsSent,
    DateTime SentDate);

using InvoiceTracker.Application.Common.Extensions;
using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Enums;
using InvoiceTracker.Domain.Interfaces;
using InvoiceTracker.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace InvoiceTracker.Application.Services;

public class InvoiceReminderService : IInvoiceReminderService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IEmailTemplateRepository _emailTemplateRepository;

    private readonly ILogger<InvoiceReminderService> logger;

    private const string testEmail = "clienttesting@gmail.com";
    public InvoiceReminderService(IInvoiceRepository invoiceRepository,
        IPersonRepository personRepository,
        IPdfGenerator pdfGenerator,
        IEmailService emailService,
        IConfiguration configuration,
        IEmailTemplateRepository emailTemplateRepository)
    {
        _invoiceRepository = invoiceRepository;
        _personRepository = personRepository;
        _pdfGenerator = pdfGenerator;
        _emailService = emailService;
        _configuration = configuration;
        _emailTemplateRepository = emailTemplateRepository;
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var invoices = await _invoiceRepository.GetOverdueInvoicesAsync(cancellationToken);
        
        logger.LogInformation("Number of overdue invoices found: {InvoiceCount}", invoices.Count);

        foreach (var invoice in invoices)
        {
            try
            {
                var customerEmail = _configuration[$"Reminder:CustomerEmails:{invoice.Reference}"];
                if (string.IsNullOrWhiteSpace(customerEmail))
                {
                    logger.LogError(
                        "Email failed for invoice {InvoiceReference} - PersonId {PersonId}",
                        invoice.Reference,
                        invoice.PersonId);
                    continue;
                }


                var client = await _personRepository.GetClientById(invoice.PersonId, cancellationToken);
                var overdueInvoice = new OverdueInvoiceDto
                {
                    InvoiceId = invoice.InvoiceId,
                    Reference = invoice.Reference,
                    InvoiceDueDate = invoice.InvoiceDueDate,
                    InvoiceAmount = invoice.InvoiceAmount,
                    CustomerEmail = testEmail,
                    CustomerName = $"{client.FullNames}".Trim()
                };

                var subject = $"Outstanding Invoice Reminder - {overdueInvoice.Reference}";

                _ = await _invoiceRepository.UpdateInvoiceStatus(invoice.InvoiceId,
                                                                InvoiceStatusEnum.Overdue,
                                                                "BackgroundWorker",
                                                                cancellationToken);
                var body = BuildReminderHtml(overdueInvoice);

                var pdf = await GenerateInvoiceToDocument(invoice.InvoiceId, cancellationToken);
                var attachment = new List<EmailAttachment>();

                if (pdf is null)
                {
                    attachment.Add(                    
                        new EmailAttachment()
                        {
                            Content = pdf,
                            ContentType = "text/html",
                            FileName = $"Invoice_{invoice.Reference}.html"
                        }
                    );
                }


                var sent = await _emailService.SendEmail(
                    testEmail,
                    subject,
                    "InvoiceStatement",
                    invoice.InvoiceId,
                    body,
                    attachment,
                    "BackgroundWorker",
                    cancellationToken);

                if (sent)
                {
                    logger.LogInformation(
                        "Email sent for invoice {InvoiceReference} to {CustomerEmail}",
                        overdueInvoice.Reference,
                        overdueInvoice.CustomerEmail);
                }
                else
                {
                    logger.LogError(
                        "Email failed for invoice {InvoiceReference} to {CustomerEmail}",
                        overdueInvoice.Reference,
                        overdueInvoice.CustomerEmail);
                }
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Email failed while processing invoice {InvoiceReference} ({InvoiceId})",
                    invoice.Reference,
                    invoice.InvoiceId);
            }            
        }
    }

    private static string BuildReminderHtml(OverdueInvoiceDto invoice) => $"""
        <!doctype html>
        <html>
        <body>
            <p>Dear {WebUtility.HtmlEncode(invoice.CustomerName)},</p>
            <p>This is a reminder that invoice <strong>{WebUtility.HtmlEncode(invoice.Reference)}</strong> is overdue.</p>
            <p>Due date: {invoice.InvoiceDueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}<br>
            Amount due: {invoice.InvoiceAmount.ToString("N2", CultureInfo.InvariantCulture)}</p>
            <p>Please find the invoice attached.</p>
        </body>
        </html>
        """;

    private static string BuildInvoiceHtml(OverdueInvoiceDto invoice) => $"""
        <!doctype html>
        <html>
        <body>
            <h1>Invoice {WebUtility.HtmlEncode(invoice.Reference)}</h1>
            <p>Customer: {WebUtility.HtmlEncode(invoice.CustomerName)}</p>
            <p>Invoice number: {WebUtility.HtmlEncode(invoice.Reference)}</p>
            <p>Due date: {invoice.InvoiceDueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}</p>
            <p>Amount due: {invoice.InvoiceAmount.ToString("N2", CultureInfo.InvariantCulture)}</p>
        </body>
        </html>
        """;

    public async Task<byte[]> GenerateInvoiceToDocument(int invoiceId, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetById(invoiceId, cancellationToken);
        var client = await _personRepository.GetClientById(invoice.PersonId, cancellationToken);

        if (invoice is null)
            return null;

        var template = await _emailTemplateRepository.GetEmailTemplate(
            EmailTemplateNamesEnum.InvoiceStatement.GetDisplayDescription(),
            cancellationToken);

        if (string.IsNullOrEmpty(template))
            return null;

        var invoiceStatus = (InvoiceStatusEnum)invoice.InvoiceStatusId;

        var invoiceDoc = await FormatInvoiceDocument(invoice, client, template, invoiceStatus);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(invoiceDoc);
        return bytes;
    }

    private async Task<string> FormatInvoiceDocument(Domain.Models.Invoice Invoice,
        Person person ,string template,
        InvoiceStatusEnum invoiceStatus)
    {
        var InvoiceDoc = template.Replace("[InvoiceNumber]", Invoice.Reference)
                                    .Replace(" [InvoiceStatus]", invoiceStatus.GetDisplayDescription())
                                    .Replace("[CustomerName]", person.FullNames)
                                    .Replace("[InvoiceDate]", Invoice.InvoiceDate.ToShortDateString())
                                    .Replace("[DueDate]", Invoice.InvoiceDueDate.ToShortDateString())
                                    .Replace("[Reference]", Invoice.Reference)
                                    .Replace("[AmountDue]", Invoice.InvoiceAmount.ToString("C2",
                                                            CultureInfo.GetCultureInfo("en-ZA")))
                                    .Replace("[Amount]", Invoice.InvoiceAmount.ToString("C2",
                                                            CultureInfo.GetCultureInfo("en-ZA")))
                                    .Replace("[InvoiceNotes]", $"Current Invoice Status{invoiceStatus.GetDisplayDescription()}");
        return InvoiceDoc;
    }
}

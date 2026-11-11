using InvoiceTracker.Application.Common.Extensions;
using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Enums;
using InvoiceTracker.Domain.Interfaces;
using InvoiceTracker.Domain.Models;
using System.Globalization;
using System.Text;
using Invoice = InvoiceTracker.Domain.Models.Invoice;

namespace InvoiceTracker.Application.Services
{
    public class InvoiceManager : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IPersonRepository _personRepository;

        public InvoiceManager(IInvoiceRepository invoiceRepository,
                                IEmailService emailService,
                                IPdfGenerator pdfGenerator,
                                IEmailTemplateRepository emailTemplateRepository,
                                IPersonRepository personRepository)
        {
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
            _pdfGenerator = pdfGenerator;
            _emailTemplateRepository = emailTemplateRepository;
            _personRepository = personRepository;
        }

        public async Task<Invoice> AddNewInvoice(CreateInvoiceRequest request, CancellationToken cancellationToken)
        {
            var invoice = MapToInvoice(request);

            var client = await _personRepository.GetClientById(request.PersonId, cancellationToken);

            if (client is null)
            {
                throw new ArgumentException($"Client with ID {request.PersonId} does not exist.");
            }

            invoice.PersonId = client.PersonId;
            invoice.PersonFullName = client.FullNames;
            invoice = await _invoiceRepository.AddInvoice(invoice, cancellationToken);
            return invoice;
        }

        public async Task<bool> EmailInvoice(
            int invoiceId,
            string recipientEmail,
            string createdBy,
            CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetById(invoiceId, cancellationToken);

            if (invoice is null)
            {
                throw new ArgumentException($"Invoice with ID {invoiceId} does not exist.");
            }

            var invoiceDoc = await GenerateInvoiceToDocument(invoiceId, cancellationToken);

            if (invoiceDoc is null)
            {
                throw new InvalidOperationException("Could not generate the invoice document.");
            }

            byte[] invoiceAttachement = Encoding.UTF8.GetBytes(invoiceDoc);

            var attachment = new List<EmailAttachment>
            {
                new EmailAttachment()
                {
                    Content = invoiceAttachement,
                    ContentType = "text/html",
                    FileName = $"Invoice_{invoice.Reference}.html"
                }
            };

            return await _emailService.SendEmail(
                    recipientEmail,
                    "Invoice Statement",
                    "InvoiceStatement",
                    invoice.InvoiceId,
                    invoiceDoc,
                    attachment,
                    createdBy,
                    cancellationToken
                );
        }

        public async Task<string> GenerateInvoiceToDocument(int invoiceId, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetById(invoiceId, cancellationToken);

            if (invoice is null)
                return null;

            var client = await _personRepository.GetClientById(invoice.PersonId,cancellationToken);
            var template = await _emailTemplateRepository.GetEmailTemplate(
                EmailTemplateNamesEnum.InvoiceStatement.GetDisplayDescription(),
                cancellationToken);

            if (string.IsNullOrEmpty(template))
                return null;

            var invoiceStatus = (InvoiceStatusEnum)invoice.InvoiceStatusId;

            var invoiceDoc = await FormatInvoiceDocument(invoice, client, template, invoiceStatus);
            
            return invoiceDoc;
        }

        public async Task<Invoice?> GetById(int invoiceId, CancellationToken cancellationToken)
        {
            var Invoice = await _invoiceRepository.GetById(invoiceId, cancellationToken);
            return Invoice ?? null;
        }

        public async Task<PagedResult<Invoice>> GetPaged(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            CancellationToken cancellationToken)
        {
            return await _invoiceRepository.GetPaged(pageNumber,
                                                    pageSize,
                                                    searchTerm,
                                                    cancellationToken);
        }

        public async Task<bool> RemoveInvoice(int invoiceId, string modifiedBy, CancellationToken cancellationToken)
        {
            return await _invoiceRepository.RemoveInvoice(invoiceId,
                                                        modifiedBy,
                                                        cancellationToken);
        }

        public async Task<bool> UpdateInvoiceStatus(int invoiceId, InvoiceStatusEnum newStatus,
            string modifiedBy, CancellationToken cancellationToken)
        {
            return await _invoiceRepository.UpdateInvoiceStatus(invoiceId,
                                                               newStatus,
                                                               modifiedBy,
                                                               cancellationToken);
        }

        public Task<bool> UpdateInvoiceStatus(int invoiceId, InvoiceStatusEnum newStatus, CancellationToken cancellationToken)
        {
            return _invoiceRepository.UpdateInvoiceStatus(
                invoiceId,
                newStatus,
                "system",
                cancellationToken);
        }

        private static Invoice MapToInvoice(CreateInvoiceRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new Invoice
            {
                PersonId = request.PersonId,
                Reference = request.Reference,
                InvoiceDate = request.InvoiceDate,
                InvoiceDueDate = request.InvoiceDueDate,
                InvoiceStatusId = (int)request.InvoiceStatusEnum,
                InvoiceAmount = request.InvoiceAmount,
                CreatedBy = request.CreatedBy,
                LineItems = (request.LineItems ?? [])
                    .Select(item =>
                    {
                        return new Domain.Models.InvoiceLineItem(0, item.Reference, item.Amount);
                    })
                    .ToList()

            };
        }

        private async Task<string> FormatInvoiceDocument(Domain.Models.Invoice Invoice,
       Person person, string template,
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
}

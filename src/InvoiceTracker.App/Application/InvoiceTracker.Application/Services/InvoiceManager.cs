using InvoiceTracker.Application.Common.Extensions;
using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Enums;
using InvoiceTracker.Domain.Interfaces;
using InvoiceTracker.Domain.Models;
using System.Globalization;
using Invoice = InvoiceTracker.Domain.Models.Invoice;

namespace InvoiceTracker.Application.Services
{
    public class InvoiceManager : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public InvoiceManager(IInvoiceRepository invoiceRepository,
                                IEmailService emailService,
                                IPdfGenerator pdfGenerator,
                                IEmailTemplateRepository emailTemplateRepository)
        {
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
            _pdfGenerator = pdfGenerator;
            _emailTemplateRepository = emailTemplateRepository;
        }

        public async Task<Invoice> AddNewInvoice(CreateInvoiceRequest request, CancellationToken cancellationToken)
        {
            var invoice = MapToInvoice(request);

            invoice = await _invoiceRepository.AddInvoice(invoice, cancellationToken);
            return invoice;
        }

        public async Task<bool> EmailInvoice(int invoiceId, int personId, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetById(invoiceId, cancellationToken);
            var pdf = await GenerateInvoiceToDocument(invoiceId, cancellationToken);


            //TODO: Cleaup the simulation codebase

            var attachment = new List<EmailAttachment>();

            attachment.Add(new EmailAttachment()
            {
                Content = pdf,
                ContentType = "application/pdf",
                FileName = $"Invoice_{invoice.Reference}.pdf"
            });

            _ = _emailService.SendEmail(
                    "mpho.moaga@outlook.com",
                    "Invoice Statement",
                    "InvoiceStatement",
                    invoice.InvoiceId,
                    "Please find attached your invoice statement.",
                    attachment,
                    "system"
                );

            return true;
        }

        public async Task<byte[]> GenerateInvoiceToDocument(int invoiceId, CancellationToken cancellationToken)
        {
            var Invoice = await _invoiceRepository.GetById(invoiceId, cancellationToken);

            if (Invoice is null)
                return null;

            var template = await _emailTemplateRepository.GetEmailTemplate(
                EmailTemplateNamesEnum.InvoiceStatement.GetDisplayDescription(),
                cancellationToken);

            if (string.IsNullOrEmpty(template))
                return null;

            var invoiceStatus = (InvoiceStatusEnum)Invoice.InvoiceStatusId;

            return  await FormatInvoicePDFDocument(Invoice, template, invoiceStatus);
        }

        public async Task<Invoice?> GetById(int invoiceId, CancellationToken cancellationToken)
        {
            var Invoice = await _invoiceRepository.GetById(invoiceId, cancellationToken);
            return Invoice ?? null;
        }

        public async Task<PagedResult<Invoice>> GetPaged(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _invoiceRepository.GetPaged(pageNumber,
                                                    pageSize,
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

        private async Task<byte[]> FormatInvoicePDFDocument(Invoice Invoice, string template, InvoiceStatusEnum invoiceStatus)
        {
            var InvoiceDoc = template.Replace("[InvoiceNumber]", Invoice.Reference)
                                        .Replace(" [InvoiceStatus]", invoiceStatus.GetDisplayDescription())
                                        .Replace("[CustomerName]", Invoice.InvoiceDueDate.ToShortDateString())
                                        .Replace("[InvoiceDate]", Invoice.InvoiceDate.ToShortDateString())
                                        .Replace("[DueDate]", Invoice.InvoiceDueDate.ToShortDateString())
                                        .Replace("[Reference]", Invoice.Reference)
                                        .Replace("[Subtotal]", Invoice.InvoiceAmount.ToString("C2",
                                                                CultureInfo.GetCultureInfo("en-ZA")));

            var pdf = await _pdfGenerator.GeneratePdfFromHTML(InvoiceDoc);

            return pdf;
        }


    }
}

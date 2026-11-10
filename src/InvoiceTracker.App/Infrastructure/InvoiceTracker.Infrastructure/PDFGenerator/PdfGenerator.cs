using InvoiceTracker.Domain.Interfaces;
//using DinkToPdf;
//using DinkToPdf.Contracts;
namespace InvoiceTracker.Infrastructure.PDFGenerator
{
    public class PdfGenerator : IPdfGenerator
    {
        //private readonly IConverter _converter;
        public PdfGenerator()
        {
            //_converter = converter;
        }

        public Task<byte[]> GeneratePdfFromHTML(string html)
        {
           throw new NotImplementedException();
        }
    }
}

using InvoiceTracker.Domain.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
namespace InvoiceTracker.Infrastructure.PDFGenerator
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IConverter _converter;
        public PdfGenerator(IConverter converter)
        {
            _converter = converter;
        }

        public Task<byte[]> GeneratePdfFromHTML(string html)
        {
            var document = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings
                    {
                        Top = 10,
                        Bottom = 10,
                        Left = 10,
                        Right = 10
                    }
                },
                Objects =
            {
                new ObjectSettings
                {
                    HtmlContent = html,
                    WebSettings = new WebSettings
                    {
                        DefaultEncoding = "utf-8"
                    }
                }
            }
            };

            var pdf = _converter.Convert(document);

            return Task.FromResult(pdf);
        }
    }
}

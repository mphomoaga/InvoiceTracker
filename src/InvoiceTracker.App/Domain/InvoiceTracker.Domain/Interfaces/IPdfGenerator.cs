namespace InvoiceTracker.Domain.Interfaces
{
    public interface IPdfGenerator
    {
        Task<byte[]> GeneratePdfFromHTML(string html);
    }
}

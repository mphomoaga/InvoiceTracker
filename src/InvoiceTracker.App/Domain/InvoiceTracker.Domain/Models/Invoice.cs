namespace InvoiceTracker.Domain.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public int PersonId { get; set; }

        public string Reference  { get; set; }

        public DateTime InvoiceDate  { get; set; }

        public DateTime InvoiceDueDate  { get; set; }

        public int InvoiceStatusId  { get; set; }

        public decimal InvoiceAmount  { get; set; }

        public string CreatedBy { get; set; }

        public string? PersonName  { get; set; }

        public List<InvoiceLineItem> LineItems  { get; set; }
    }
}

using System.ComponentModel;

namespace InvoiceTracker.Domain.Enums
{
    public enum EmailTemplateNamesEnum
    {
        [Description("InvoiceStatement")]
        InvoiceStatement = 1,
        [Description("InvoiceStatementMessage")]
        InvoiceStatementMessage = 2,
        [Description("OverduePaymentEmailMessage")]
        OverduePaymentEmailMessage = 3
    }
}

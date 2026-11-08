using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTracker.Infrastructure.Repository
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly InvoiceTrackerDbContext _context;

        public EmailTemplateRepository(InvoiceTrackerDbContext context) => _context = context;
        public async Task<string> GetEmailTemplate(string templateName,
            CancellationToken token)
        {
            var tamplate =  await _context.EmailTemplates
                .AsNoTracking().
                FirstOrDefaultAsync(template => template.TemplateName == templateName, token);

            return tamplate != null ? tamplate.Value : string.Empty;
        }
    }
}

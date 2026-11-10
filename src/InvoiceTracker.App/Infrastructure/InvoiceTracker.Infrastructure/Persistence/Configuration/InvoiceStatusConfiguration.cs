using InvoiceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceTracker.Infrastructure.Persistence.Configuration
{
    internal class InvoiceStatusConfiguration : IEntityTypeConfiguration<InvoiceStatus>
    {
        public void Configure(EntityTypeBuilder<InvoiceStatus> builder)
        {
            builder.ToTable("InvoiceStatus","common");


            builder.HasKey(x => x.InvoiceStatusId);

            builder.Property(x => x.InvoiceStatusId)
                .ValueGeneratedNever();

            builder.Property(x => x.Description)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasData(
                new InvoiceStatus { InvoiceStatusId = 1, Description = "Draft" },
                new InvoiceStatus { InvoiceStatusId = 2, Description = "Pending" },
                new InvoiceStatus { InvoiceStatusId = 3, Description = "Sent" },
                new InvoiceStatus { InvoiceStatusId = 4, Description = "Viewed" },
                new InvoiceStatus { InvoiceStatusId = 5, Description = "Partially Paid" },
                new InvoiceStatus { InvoiceStatusId = 6, Description = "Paid" },
                new InvoiceStatus { InvoiceStatusId = 7, Description = "Overdue" },
                new InvoiceStatus { InvoiceStatusId = 8, Description = "Cancelled" },
                new InvoiceStatus { InvoiceStatusId = 9, Description = "Refunded" },
                new InvoiceStatus { InvoiceStatusId = 10, Description = "Written Off" }
            );
        }
    }
}



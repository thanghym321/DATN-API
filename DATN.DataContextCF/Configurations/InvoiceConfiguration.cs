using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DataContextCF.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoice");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.UserId).HasColumnType("int").IsRequired();
            builder.Property(x => x.RoomId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DateCreated).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(x => x.DatePayment).HasColumnType("datetime");
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValue(StatusInvoice.unpaid);

            builder.HasOne<Users>(x => x.User).WithMany(x => x.Invoices).HasForeignKey(x => x.UserId);
            builder.HasOne<Room>(x => x.Room).WithMany(x => x.Invoices).HasForeignKey(x => x.RoomId);
            builder.HasMany<UtilityBill>(x => x.UtilityBills).WithOne(x => x.Invoice).HasForeignKey(x => x.InvoiceId);

        }
    }
}
using DATN.DataContextCF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DataContextCF.Configurations
{
    public class UtilityBillConfiguration : IEntityTypeConfiguration<UtilityBill>
    {
        public void Configure(EntityTypeBuilder<UtilityBill> builder)
        {
            builder.ToTable("UtilityBill");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.InvoiceId).HasColumnType("int").IsRequired();
            builder.Property(x => x.Type).HasColumnType("nvarchar(50)");
            builder.Property(x => x.Quantity).HasColumnType("decimal").IsRequired();
            builder.Property(x => x.UnitPrice).HasColumnType("decimal").IsRequired();

            builder.HasOne<Invoice>(x => x.Invoice).WithMany(x => x.UtilityBills).HasForeignKey(x => x.InvoiceId);
        }
    }
}

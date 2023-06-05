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
    public class ElectricityWaterRateConfiguration : IEntityTypeConfiguration<ElectricityWaterRate>
    {
        public void Configure(EntityTypeBuilder<ElectricityWaterRate> builder)
        {
            builder.ToTable("ElectricityWaterRate");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.Type).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.Tier).HasColumnType("int").IsRequired();
            builder.Property(x => x.StartAmount).HasColumnType("int");
            builder.Property(x => x.EndAmount).HasColumnType("int");
            builder.Property(x => x.Unit).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal").IsRequired();

        }
    }
}

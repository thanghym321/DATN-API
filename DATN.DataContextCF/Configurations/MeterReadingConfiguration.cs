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
    public class MeterReadingConfiguration : IEntityTypeConfiguration<MeterReading>
    {
        public void Configure(EntityTypeBuilder<MeterReading> builder)
        {
            builder.ToTable("MeterReading");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.RoomId).HasColumnType("int").IsRequired();
            builder.Property(x => x.ElectricityIndex).HasColumnType("int").IsRequired();
            builder.Property(x => x.WaterIndex).HasColumnType("int").IsRequired();
            builder.Property(x => x.Date).HasColumnType("datetime").HasDefaultValueSql("getdate()");

            builder.HasOne<Room>(x => x.Room).WithMany(x => x.MeterReadings).HasForeignKey(x => x.RoomId);
        }
    }
}

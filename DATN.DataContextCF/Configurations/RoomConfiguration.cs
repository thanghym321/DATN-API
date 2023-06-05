using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DataContextCF.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Room");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.BuildingId).HasColumnType("int");
            builder.Property(x => x.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.Type).HasColumnType("nvarchar(50)");
            builder.Property(x => x.Bed).HasColumnType("int");
            builder.Property(x => x.Price).HasColumnType("decimal");
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValue(StatusRoom.available);

            builder.HasOne<Building>(x => x.Building).WithMany(x => x.Rooms).HasForeignKey(x => x.BuildingId);
            builder.HasMany<MeterReading>(x => x.MeterReadings).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
            builder.HasMany<RoomRegistration>(x => x.RoomRegistrations).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
        }
    }
}

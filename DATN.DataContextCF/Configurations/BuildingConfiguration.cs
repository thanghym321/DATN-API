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
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {

        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.ToTable("Building");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.CampusId).HasColumnType("int").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.Floor).HasColumnType("int").IsRequired();
            builder.Property(x => x.Room).HasColumnType("int").IsRequired();


            builder.HasOne<Campus>(x => x.Campus).WithMany(x => x.Buildings).HasForeignKey(x => x.CampusId);
            builder.HasMany<Room>(x => x.Rooms).WithOne(x => x.Building).HasForeignKey(x => x.BuildingId);
        }
    }
}

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
    public class CampusConfiguration : IEntityTypeConfiguration<Campus>
    {
        public void Configure(EntityTypeBuilder<Campus> builder)
        {
            builder.ToTable("Campus");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.Address).HasColumnType("nvarchar(250)");
            builder.Property(x => x.Email).HasColumnType("varchar(100)");
            builder.Property(x => x.Phone).HasColumnType("varchar(11)");


            builder.HasMany<Building>(x => x.Buildings).WithOne(x => x.Campus).HasForeignKey(x => x.CampusId);
        }
    }
}

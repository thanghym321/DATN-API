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
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Service");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.Description).HasColumnType("ntext");
            builder.Property(x => x.Unit).HasColumnType("nvarchar(50)").IsRequired(); ;
            builder.Property(x => x.Price).HasColumnType("decimal").IsRequired(); ;

            builder.HasMany<ServiceRegistration>(x => x.ServiceRegistrations).WithOne(x => x.Service).HasForeignKey(x => x.ServiceId);
        }
    }
}

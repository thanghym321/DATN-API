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
    public class ServiceRegistrationConfiguration : IEntityTypeConfiguration<ServiceRegistration>
    {
        public void Configure(EntityTypeBuilder<ServiceRegistration> builder)
        {
            builder.ToTable("ServiceRegistration");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.ServiceId).HasColumnType("int").IsRequired();
            builder.Property(x => x.UserId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DateRegistration).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValue(StatusReg.wait);

            builder.HasOne<Service>(x => x.Service).WithMany(x => x.ServiceRegistrations).HasForeignKey(x => x.ServiceId);
            builder.HasOne<Users>(x => x.User).WithMany(x => x.ServiceRegistrations).HasForeignKey(x => x.UserId);
        }
    }
}

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
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.BuildingId).HasColumnType("int");
            builder.Property(x => x.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.DateOfBirth).HasColumnType("datetime");
            builder.Property(x => x.Gender).HasColumnType("nvarchar(3)");
            builder.Property(x => x.Avatar).HasColumnType("ntext");
            builder.Property(x => x.Address).HasColumnType("nvarchar(250)");
            builder.Property(x => x.Email).HasColumnType("varchar(100)");
            builder.Property(x => x.Phone).HasColumnType("varchar(11)");
            builder.Property(x => x.CitizenIdentityCard).HasColumnType("char(12)");

            //mối quan hệ một - một
            builder.HasOne<Account>(x => x.Account).WithOne(x => x.User).HasForeignKey<Account>(x => x.UserId);

            builder.HasMany<Feedback>(x => x.Feedbacks).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany<Invoice>(x => x.Invoices).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany<Report>(x => x.Reports).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany<RoomRegistration>(x => x.RoomRegistrations).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany<ServiceRegistration>(x => x.ServiceRegistrations).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}

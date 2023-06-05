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
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1,1);
            builder.Property(x => x.UserName).HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.PassWord).HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValue(Status.Active);
            builder.Property(x => x.Role).HasColumnType("int").HasDefaultValue(Role.Student);

            //mối quan hệ một - một
            builder.HasOne<Users>(x => x.User).WithOne(x => x.Account).HasForeignKey<Account>(x => x.UserId);
        }
    }
}

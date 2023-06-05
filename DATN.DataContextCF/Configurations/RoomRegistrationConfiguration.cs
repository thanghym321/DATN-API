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
    public class RoomRegistrationConfiguration : IEntityTypeConfiguration<RoomRegistration>
    {
        public void Configure(EntityTypeBuilder<RoomRegistration> builder)
        {
            builder.ToTable("RoomRegistration");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.RoomId).HasColumnType("int").IsRequired();
            builder.Property(x => x.UserId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DateRegistration).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValue(StatusReg.wait);

            builder.HasOne<Room>(x => x.Room).WithMany(x => x.RoomRegistrations).HasForeignKey(x => x.RoomId);
            builder.HasOne<Users>(x => x.User).WithMany(x => x.RoomRegistrations).HasForeignKey(x => x.UserId);
        }
    }
}

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
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedback");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.UserId).HasColumnType("int").IsRequired();
            builder.Property(x => x.ReportId).HasColumnType("int").IsRequired();
            builder.Property(x => x.Title).HasColumnType("nvarchar(250)").IsRequired();
            builder.Property(x => x.Content).HasColumnType("ntext").IsRequired();
            builder.Property(x => x.DateCreated).HasColumnType("datetime").HasDefaultValueSql("getdate()");

            builder.HasOne<Users>(x => x.User).WithMany(x => x.Feedbacks).HasForeignKey(x => x.UserId);
            builder.HasOne<Report>(x => x.Report).WithOne(x => x.Feedback).HasForeignKey<Feedback>(x => x.ReportId).OnDelete(DeleteBehavior.NoAction); ;
        }
    }
}

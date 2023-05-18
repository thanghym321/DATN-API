using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class DATN_DFContext : DbContext
    {
        public DATN_DFContext()
        {
        }

        public DATN_DFContext(DbContextOptions<DATN_DFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Campus> Campuses { get; set; }
        public virtual DbSet<DetailPayment> DetailPayments { get; set; }
        public virtual DbSet<ElectricityWaterRate> ElectricityWaterRates { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<MeterReading> MeterReadings { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomRegistration> RoomRegistrations { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceRegistration> ServiceRegistrations { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ANHTHAWNGS\\SQLEXPRESS;Database=DATN_DF;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("passWord");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fkAccount_UserId");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CampusId).HasColumnName("campusId");

                entity.Property(e => e.Floor).HasColumnName("floor");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Room).HasColumnName("room");

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Buildings)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("fkBuilding_CampusId");
            });

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.ToTable("Campus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DetailPayment>(entity =>
            {
                entity.ToTable("DetailPayment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PaymentId).HasColumnName("paymentId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.RandomId).HasColumnName("randomId");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("total");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.DetailPayments)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("fkDetailPayment_PaymentId");
            });

            modelBuilder.Entity<ElectricityWaterRate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AmountUsed).HasColumnName("amountUsed");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fkFeedback_UserId");
            });

            modelBuilder.Entity<MeterReading>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.ElectricityIndex).HasColumnName("electricityIndex");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.WaterIndex).HasColumnName("waterIndex");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.MeterReadings)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("fkMeterReadings_RoomId");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DatePayment)
                    .HasColumnType("date")
                    .HasColumnName("datePayment");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fkPayment_UserId");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fkReport_UserId");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bed).HasColumnName("bed");

                entity.Property(e => e.BuildingId).HasColumnName("buildingId");

                entity.Property(e => e.SemesterPrice)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("semesterPrice");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.SummerPrice)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("summerPrice");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("fkRoom_BuildingId");
            });

            modelBuilder.Entity<RoomRegistration>(entity =>
            {
                entity.ToTable("RoomRegistration");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateRegistration)
                    .HasColumnType("date")
                    .HasColumnName("dateRegistration");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomRegistrations)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("fkRoomRegistration_RoomId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RoomRegistrations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fkRoomRegistration_UserId");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .HasColumnName("unit");
            });

            modelBuilder.Entity<ServiceRegistration>(entity =>
            {
                entity.ToTable("ServiceRegistration");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateRegistration)
                    .HasColumnType("date")
                    .HasColumnName("dateRegistration");

                entity.Property(e => e.ServiceId).HasColumnName("serviceId");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceRegistrations)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("fkServiceRegistration_ServiceId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServiceRegistrations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fkServiceRegistration_UserId");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .HasColumnName("address");

                entity.Property(e => e.Avatar)
                    .HasColumnType("ntext")
                    .HasColumnName("avatar");

                entity.Property(e => e.CampusId).HasColumnName("campusId");

                entity.Property(e => e.CitizenIdentityCard)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("citizenIdentityCard")
                    .IsFixedLength(true);

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Gender)
                    .HasMaxLength(3)
                    .HasColumnName("gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

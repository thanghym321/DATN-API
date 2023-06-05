using DATN.DataContextCF.Configurations;
using DATN.DataContextCF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DataContextCF.EF
{
    public class DATN_CFContext  : DbContext
    {
        public DATN_CFContext(DbContextOptions<DATN_CFContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());
            modelBuilder.ApplyConfiguration(new CampusConfiguration());
            modelBuilder.ApplyConfiguration(new UtilityBillConfiguration());
            modelBuilder.ApplyConfiguration(new ElectricityWaterRateConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
            modelBuilder.ApplyConfiguration(new MeterReadingConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RoomRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());

            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<UtilityBill> UtilityBills { get; set; }
        public DbSet<ElectricityWaterRate> ElectricityWaterRates { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomRegistration> RoomRegistrations { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceRegistration> ServiceRegistrations { get; set; }
        public DbSet<Users> Users { get; set; }
    }

}

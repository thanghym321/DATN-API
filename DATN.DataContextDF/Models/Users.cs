using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Users
    {
        public Users()
        {
            Accounts = new HashSet<Account>();
            Feedbacks = new HashSet<Feedback>();
            Payments = new HashSet<Payment>();
            Reports = new HashSet<Report>();
            RoomRegistrations = new HashSet<RoomRegistration>();
            ServiceRegistrations = new HashSet<ServiceRegistration>();
        }

        public int Id { get; set; }
        public int? CampusId { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CitizenIdentityCard { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<RoomRegistration> RoomRegistrations { get; set; }
        public virtual ICollection<ServiceRegistration> ServiceRegistrations { get; set; }
    }
}

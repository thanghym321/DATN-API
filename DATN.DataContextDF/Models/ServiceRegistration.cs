using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class ServiceRegistration
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateRegistration { get; set; }
        public string Status { get; set; }

        public virtual Service Service { get; set; }
        public virtual Users User { get; set; }
    }
}

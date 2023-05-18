using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class RoomRegistration
    {
        public int Id { get; set; }
        public int? RoomId { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateRegistration { get; set; }
        public string Status { get; set; }

        public virtual Room Room { get; set; }
        public virtual Users User { get; set; }
    }
}

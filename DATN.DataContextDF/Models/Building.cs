using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Building
    {
        public Building()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public int? CampusId { get; set; }
        public string Name { get; set; }
        public int? Floor { get; set; }
        public int? Room { get; set; }

        public virtual Campus Campus { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}

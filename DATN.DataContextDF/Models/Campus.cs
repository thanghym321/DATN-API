using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Campus
    {
        public Campus()
        {
            Buildings = new HashSet<Building>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Building> Buildings { get; set; }
    }
}

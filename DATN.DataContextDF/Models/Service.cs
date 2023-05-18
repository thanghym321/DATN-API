using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Service
    {
        public Service()
        {
            ServiceRegistrations = new HashSet<ServiceRegistration>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<ServiceRegistration> ServiceRegistrations { get; set; }
    }
}

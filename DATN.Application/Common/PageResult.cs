using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.Common
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalItem { get; set; }
    }
}

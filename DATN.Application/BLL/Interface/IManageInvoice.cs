using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageInvoice
    {
        public void Start();
        public Task RoomInvoices();
        public Task ElectricityWaterInvoices();
        public Task ElectricityWaterMail(string type, string Name, int Id, string RoomName, DateTime DateCreated, decimal Total);
        public Task<decimal> Calculated(int AmountUsed, string Type);
    }
}

using DATN.Application.Common;
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
        public Task<InvoiceViewModel> GetById(int Id);
        public Task<PageResult<InvoiceViewModel>> GetAllPaging(RoomInvoiceModel request);
        public Task RoomInvoices();
        public Task ElectricityWaterInvoices();
        public Task SendEmail(string type, string Name, int Id, string RoomName, DateTime Date, DateTime DatePre, decimal Total);
        public Task<decimal> Calculated(int AmountUsed, string Type);
    }
}

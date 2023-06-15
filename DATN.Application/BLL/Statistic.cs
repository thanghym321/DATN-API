using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL
{
    public class Statistic :IStatistic
    {
        private readonly DATN_CFContext _context;
        public Statistic(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<int> StatisticCampus()
        {
            var TotalCampus = await _context.Campuses.CountAsync();

            return TotalCampus;
        }
        public async Task<int> StatisticBuilding()
        {
            var TotalBuilding = await _context.Buildings.CountAsync();

            return TotalBuilding;

        }
        public async Task<StatisticRoom> StatisticRoom()
        {
            var query = _context.Rooms;

            int TotalRoom = await query.CountAsync();
            var AvailableRoom =  await query.CountAsync(x => x.Status==(int)StatusRoom.available);
            var FullRoom = await query.CountAsync(x => x.Status == (int)StatusRoom.full);


            return new StatisticRoom()
            {
                TotalRoom = TotalRoom,
                AvailableRoom = AvailableRoom,
                FullRoom = FullRoom
            };

        }
        public async Task<int> StatisticStudent()
        {
            var TotalStudent = await _context.Accounts.CountAsync(x => x.Role == (int)Role.Student);

            return TotalStudent;
        }
        public async Task<StatisticInvoice> StatisticInvoice()
        {
            var query = from a in _context.Invoices
                        join b in _context.UtilityBills on a.Id equals b.InvoiceId
                        select new { a, b };

            int TotalInvoice = await query.CountAsync();
            int TotalInvoiceUnpaid = await query.CountAsync(x => x.a.Status == (int)StatusInvoice.unpaid);
            int TotalInvoicePaid = await query.CountAsync(x => x.a.Status == (int)StatusInvoice.paid);
            decimal TotalRoomPrice = query.Where(x => x.b.Type=="Phòng" && x.a.Status==(int)StatusInvoice.paid).Sum(x => x.a.Total);
            decimal TotalElecPrice = query.Where(x => x.b.Type == "Điện" && x.a.Status == (int)StatusInvoice.paid).Sum(x => x.a.Total);
            decimal TotalWaterPrice = query.Where(x => x.b.Type == "Nước" && x.a.Status == (int)StatusInvoice.paid).Sum(x => x.a.Total);

            return new StatisticInvoice()
            {
                TotalInvoice = TotalInvoice,
                TotalInvoiceUnpaid = TotalInvoiceUnpaid,
                TotalInvoicePaid = TotalInvoicePaid,
                TotalRoomPrice = TotalRoomPrice,
                TotalElecPrice = TotalElecPrice,
                TotalWaterPrice = TotalWaterPrice
            };
        }
    }
    public class StatisticRoom
    {
        public int TotalRoom { get; set; }
        public int AvailableRoom { get; set; }
        public int FullRoom { get; set; }
    }
    public class StatisticInvoice
    {
        public int TotalInvoice { get; set;}
        public int TotalInvoiceUnpaid { get; set; }
        public int TotalInvoicePaid { get; set; }
        public decimal TotalRoomPrice { get; set; }
        public decimal TotalElecPrice { get; set; }
        public decimal TotalWaterPrice { get; set; }

    }
}

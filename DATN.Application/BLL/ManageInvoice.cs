using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DATN.Application.BLL
{
    public class ManageInvoice :IManageInvoice
    {
        private readonly DATN_CFContext _context;
        private readonly ISendMailService _sendMailService;
        private Timer _timer;
        public ManageInvoice(DATN_CFContext context, ISendMailService sendMailService)
        {
            _context = context;
            _sendMailService = sendMailService;
        }

        public void Start()
        {
            DateTime now = DateTime.Now;
            DateTime nextMonth = now.AddMonths(1).Date;
            DateTime firstDayOfMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            TimeSpan timeRemaining = firstDayOfMonth - now;

            Timer timer = new Timer(async state =>
            {
                await RoomInvoices();
                await ElectricityWaterInvoices();
            }, null, TimeSpan.FromSeconds(10), timeRemaining);
        }

        public async Task RoomInvoices()
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        select new { a, b, c };

            //danh sách đăng ký phòng đã xác nhận
            var listreg = query.Where(x => x.a.Status==(int)StatusReg.confirm).ToList();

            //duyệt qua danh sách phòng đã xác nhận
            foreach(var item in listreg)
            {
                //đếm số đơn đăng ký từ phòng nay để ra số sinh viên
                int totaluser = listreg.Where(x => x.a.RoomId == item.c.Id).Count();
                //giá chia theo đâu người
                decimal totalprice= item.c.Price/totaluser;
                Invoice invoice = new Invoice();
                invoice.UserId = item.a.UserId;
                invoice.RoomId = item.a.RoomId;
                invoice.Total = totalprice;
                invoice.Status = (int)StatusInvoice.unpaid;
                await _context.AddAsync(invoice);
                await _context.SaveChangesAsync();

                var email = "thanghym321@gmail.com";
                var subject = "Thông báo hóa đơn tiền phòng hàng tháng";
                var htmlMessage =  "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  " +
                    "<title>Thông báo hóa đơn tiền phòng hàng tháng</title>\r\n</head>\r\n<body>\r\n  " +
                    "<h1>Thông báo hóa đơn tiền phòng hàng tháng</h1>\r\n  " +
                    "<p>Xin chào " + item.b.Name + ",</p>\r\n  \r\n  " +
                    "<p>Chúng tôi xin thông báo hóa đơn tiền phòng tháng " + DateTime.Now.AddMonths(-1).Month + ".</p>\r\n  \r\n  " +
                    "<p>Thông tin hóa đơn:</p>\r\n  <ul>\r\n    " +
                    "<li><strong>Số hóa đơn:</strong> " + invoice.Id + " </li>\r\n    " +
                    "<li><strong>Họ và tên:</strong>" + item.b.Name + " </li>\r\n   " +
                    " <li><strong>Phòng:</strong> " + item.c.Name + "</li>\r\n    " +
                    "<li><strong>Ngày:</strong> " + invoice.DateCreated + " </li>\r\n    " +
                    "<li><strong>Số tiền:</strong> " + invoice.Total.ToString("N0") + " VND </li>\r\n    " +
                    "<p>Trân trọng,</p>\r\n  KTX UTEHY </p>\r\n</body>\r\n</html>";

                await _sendMailService.SendEmailAsync(email, subject, htmlMessage);
            }
        }

        public async Task ElectricityWaterInvoices()
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        join d in _context.MeterReadings on c.Id equals d.RoomId
                        select new { a, b, c , d };

            //lấy ra list theo tháng ghi số điện, tháng này và tháng trước
            var list = await query.Where(x => x.d.Date.Month==DateTime.Now.Month).ToListAsync();
            var listpre = await query.Where(x => x.d.Date.Month == DateTime.Now.AddMonths(-1).Month).ToListAsync();

            //duyệt qua danh sách
            foreach (var item in query)
            {
                //tính số điện và nước sử dụng, tháng này - tháng trước
                int elec = list.Where(x => x.c.Id == item.c.Id).Select(x => x.d.ElectricityIndex).FirstOrDefault();
                int elecpre = listpre.Where(x => x.c.Id == item.c.Id).Select(x => x.d.ElectricityIndex).FirstOrDefault();
                int water = list.Where(x => x.c.Id == item.c.Id).Select(x => x.d.WaterIndex).FirstOrDefault();
                int waterpre = listpre.Where(x => x.c.Id == item.c.Id).Select(x => x.d.WaterIndex).FirstOrDefault();

                int elecUsed = elec - elecpre;
                int waterUsed = water - waterpre;

                //gọi hàm tính tiền điện nước
                decimal elecTotal = await Calculated(elecUsed,"Điện");
                decimal waterTotal = await Calculated(waterUsed,"Nước");

                //đếm số đơn đăng ký đươc xác nhận từ phòng nay để ra số sinh viên
                int totaluser = query.Where(x => x.a.RoomId == item.c.Id).Count();

                //tạo hóa đơn tiền điện
                Invoice elecInvoice = new Invoice();
                elecInvoice.UserId = item.a.UserId;
                elecInvoice.RoomId = item.a.RoomId;
                elecInvoice.Total = elecTotal;
                elecInvoice.Status = (int)StatusInvoice.unpaid;
                await _context.AddAsync(elecInvoice);

                UtilityBill elecUti = new UtilityBill();
                elecUti.InvoiceId = elecInvoice.Id;
                elecUti.type = "Điện";
                elecUti.Quantity = elecUsed;
                elecUti.UnitPrice = elecTotal / elecUsed;
                await _context.AddAsync(elecUti);

                //tạo hóa đơn tiền nước
                Invoice waterInvoice = new Invoice();
                waterInvoice.UserId = item.a.UserId;
                waterInvoice.RoomId = item.a.RoomId;
                waterInvoice.Total = waterTotal;
                waterInvoice.Status = (int)StatusInvoice.unpaid;
                await _context.AddAsync(waterInvoice);

                UtilityBill waterUti = new UtilityBill();
                waterUti.InvoiceId = elecInvoice.Id;
                waterUti.type = "Nước";
                waterUti.Quantity = waterUsed;
                waterUti.UnitPrice = waterTotal / waterUsed;
                await _context.AddAsync(waterUti);

                await _context.SaveChangesAsync();

                await ElectricityWaterMail("Điện", item.b.Name, elecInvoice.Id, item.c.Name, elecInvoice.DateCreated, elecTotal);
                await ElectricityWaterMail("Nước", item.b.Name, waterInvoice.Id, item.c.Name, waterInvoice.DateCreated, waterTotal);
            }
        }

        public async Task ElectricityWaterMail(string type,string Name, int Id, string RoomName, DateTime DateCreated, decimal Total)
        {
            var email = "thanghym321@gmail.com";
            var subject = "Thông báo hóa đơn tiền " + type + " hàng tháng";
            var htmlMessage = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  " +
                "<title>Thông báo hóa đơn tiền " + type + " hàng tháng</title>\r\n</head>\r\n<body>\r\n  " +
                "<h1>Thông báo hóa đơn tiền " + type + " hàng tháng</h1>\r\n  " +
                "<p>Xin chào " + Name + ",</p>\r\n  \r\n  " +
                "<p>Chúng tôi xin thông báo hóa đơn tiền " + type + " tháng " + DateTime.Now.AddMonths(-1).Month + ".</p>\r\n  \r\n  " +
                "<p>Thông tin hóa đơn:</p>\r\n  <ul>\r\n    " +
                "<li><strong>Số hóa đơn:</strong> " + Id + " </li>\r\n    " +
                "<li><strong>Họ và tên:</strong>" + Name + " </li>\r\n   " +
                " <li><strong>Phòng:</strong> " + RoomName + "</li>\r\n    " +
                "<li><strong>Ngày:</strong> " + DateCreated + " </li>\r\n    " +
                "<li><strong>Số tiền:</strong> " + Total.ToString("N0") + " VND </li>\r\n    " +
                "<p>Trân trọng,</p>\r\n  KTX UTEHY </p>\r\n</body>\r\n</html>";

            await _sendMailService.SendEmailAsync(email, subject, htmlMessage);
        }

        public async Task<decimal> Calculated(int AmountUsed,string Type)
        {
            decimal total = 0;
            int remainingConsumption = AmountUsed;

            var rates = await _context.ElectricityWaterRates.Where(x => x.Type == Type).ToListAsync();

            foreach (var x in rates)
            {
                if (remainingConsumption <= 0)
                    break;

                int tierConsumption = Math.Min(remainingConsumption, x.EndAmount - x.StartAmount + 1);
                total += tierConsumption * x.Price;
                remainingConsumption -= tierConsumption;
            }
            return total;
        }
    }
}

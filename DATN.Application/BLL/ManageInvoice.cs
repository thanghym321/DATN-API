using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.Application.User;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
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
            }, null, timeRemaining, timeRemaining);
        }

        public async Task<InvoiceViewModel> GetById(int Id)
        {
            var query = from a in _context.Invoices
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        join d in _context.UtilityBills on a.Id equals d.InvoiceId
                        select new InvoiceViewModel
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Name = b.Name,
                            RoomId = a.RoomId,
                            RoomName = c.Name,
                            DateCreated = a.DateCreated,
                            DatePayment = a.DatePayment,
                            Total = a.Total,
                            Status = a.Status,
                            Type = d.Type,
                            Quantity = d.Quantity,
                            UnitPrice = d.UnitPrice,
                        };

            var obj = await query.FirstOrDefaultAsync(x => x.Id == Id);

            return obj;
        }

        public async Task<PageResult<InvoiceViewModel>> GetAllPaging(RoomInvoiceModel request)
        {
            var query = from a in _context.Invoices
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        join d in _context.UtilityBills on a.Id equals d.InvoiceId
                        join e in _context.Buildings on c.BuildingId equals e.Id
                        join f in _context.Campuses on e.CampusId equals f.Id
                        select new { a, b , c , d , e , f };

            var student = _context.Accounts.Where(x => x.UserId == request.Id).FirstOrDefault();

            if (student.Role==(int)Role.Student) { 
                query = query.Where(x => x.a.UserId== request.Id);
            }
            if (request.Low.HasValue && request.High.HasValue)
            {
                query = query.Where(x => x.a.Total >= request.Low && x.a.Total <= request.High);
            }
            if (request.Month.HasValue)
            {
                query = query.Where(x => x.a.DateCreated.Month == request.Month);
            }
            if (request.Year.HasValue)
            {
                query = query.Where(x => x.a.DateCreated.Year == request.Year);
            }
            if (!string.IsNullOrEmpty(request.Type))
            {
                query = query.Where(x => x.d.Type == request.Type);
            }
            if (request.Status != -1)
            {
                query = query.Where(x => x.a.Status == request.Status);
            }
            if (request.Filter == "")
            {
                query = query.OrderByDescending(x => x.a.DateCreated);

            }
            if (request.Filter == "high")
            {
                query = query.OrderByDescending(x => x.a.Total);

            }
            if (request.Filter == "low")
            {
                query = query.OrderBy(x => x.a.Total);

            }
            if (request.Filter == "new")
            {
                query = query.OrderByDescending(x => x.a.DateCreated);

            }
            if (request.Filter == "old")
            {
                query = query.OrderBy(x => x.a.DateCreated);

            }
            if (request.BuildingId != -1)
            {
                query = query.Where(x => x.e.Id== request.BuildingId);
            }
            if (request.CampusId != -1)
            {
                query = query.Where(x => x.f.Id == request.CampusId);
            }


            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageindex - 1) * request.pagesize).Take(request.pagesize)
            .Select(x => new InvoiceViewModel()
            {
                Id = x.a.Id,
                UserId = x.a.UserId,
                Name = x.b.Name,
                Email = x.b.Email,
                Phone = x.b.Phone,
                Address = x.b.Address,
                RoomId = x.a.RoomId,
                RoomName = x.c.Name,
                DateCreated = x.a.DateCreated,
                DatePayment = x.a.DatePayment,
                Total = x.a.Total,
                Status = x.a.Status,
                Type = x.d.Type,
                Quantity = x.d.Quantity,
                UnitPrice = x.d.UnitPrice,
                BuildingId = x.e.Id,
                BuildingName = x.e.Name,
                CampusId = x.f.Id,
                CampusName = x.f.Name,

            }).ToListAsync();

            var pageResult = new PageResult<InvoiceViewModel>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;
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


                UtilityBill roomUti = new UtilityBill();
                roomUti.InvoiceId = invoice.Id;
                roomUti.Type = "Phòng";
                roomUti.Quantity = 1;
                roomUti.UnitPrice = totalprice;
                await _context.AddAsync(roomUti);
                await _context.SaveChangesAsync();

                await SendEmail("Phòng", item.b.Name, invoice.Id, item.c.Name, invoice.DateCreated, invoice.DateCreated.AddMonths(-1), totalprice);

            }
        }

        public async Task ElectricityWaterInvoices()
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        join d in _context.MeterReadings on c.Id equals d.RoomId
                        select new { a, b, c , d };

            //lấy ra list theo tháng ghi số điện, tháng này và tháng trước (chỉ số tháng này phải lớn hơn 0)
            var list = await query.Where(x => x.d.Date.Month==DateTime.Now.Month 
                                        && x.d.ElectricityIndex > 0 
                                        && x.d.WaterIndex > 0
                                        && x.a.Status==(int)StatusReg.confirm
                                        && x.d.Date.Month == DateTime.Now.AddMonths(-1).Month
                                        ).ToListAsync();
            var listpre = await query.Where(x => x.d.Date.Month == DateTime.Now.AddMonths(-2).Month).ToListAsync();

            //duyệt qua danh sách
            foreach (var item in list)
            {
                //ngày ghi tháng trước và tháng này 
                DateTime Date = list.Where(x => x.c.Id == item.c.Id).Select(x => x.d.Date).FirstOrDefault();
                DateTime DatePre = listpre.Where(x => x.c.Id == item.c.Id).Select(x => x.d.Date).DefaultIfEmpty(Date.AddMonths(-1)).FirstOrDefault();

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

                //đếm số sinh viên trong phòng
                int totaluser = query.Where(x => x.a.RoomId == item.c.Id && x.a.Status==(int)StatusReg.confirm).Count();

                //tạo hóa đơn tiền điện
                Invoice elecInvoice = new Invoice();
                elecInvoice.UserId = item.a.UserId;
                elecInvoice.RoomId = item.a.RoomId;
                elecInvoice.Total = elecTotal;
                elecInvoice.Status = (int)StatusInvoice.unpaid;
                await _context.AddAsync(elecInvoice);
                await _context.SaveChangesAsync();


                UtilityBill elecUti = new UtilityBill();
                elecUti.InvoiceId = elecInvoice.Id;
                elecUti.Type = "Điện";
                elecUti.Quantity = elecUsed;
                elecUti.UnitPrice = elecTotal / elecUsed;
                await _context.AddAsync(elecUti);
                await _context.SaveChangesAsync();


                //tạo hóa đơn tiền nước
                Invoice waterInvoice = new Invoice();
                waterInvoice.UserId = item.a.UserId;
                waterInvoice.RoomId = item.a.RoomId;
                waterInvoice.Total = waterTotal;
                waterInvoice.Status = (int)StatusInvoice.unpaid;
                await _context.AddAsync(waterInvoice);
                await _context.SaveChangesAsync();


                UtilityBill waterUti = new UtilityBill();
                waterUti.InvoiceId = elecInvoice.Id;
                waterUti.Type = "Nước";
                waterUti.Quantity = waterUsed;
                waterUti.UnitPrice = waterTotal / waterUsed;
                await _context.AddAsync(waterUti);
                await _context.SaveChangesAsync();


                await SendEmail("Điện", item.b.Name, elecInvoice.Id, item.c.Name, Date, DatePre, elecTotal);
                await SendEmail("Nước", item.b.Name, waterInvoice.Id, item.c.Name, Date, DatePre, waterTotal);

            }
        }

        public async Task SendEmail(string Type,string Name, int Id, string RoomName, DateTime Date, DateTime DatePre , decimal Total)
        {
            var email = "thanghym321@gmail.com";
            var subject = "Thông báo hóa đơn tiền " + Type + " hàng tháng";
            var htmlMessage = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  " +
                "<title>Thông báo hóa đơn tiền " + Type + " hàng tháng</title>\r\n</head>\r\n<body>\r\n  " +
                "<h1>Thông báo hóa đơn tiền " + Type + " hàng tháng</h1>\r\n  " +
                "<p>Xin chào " + Name + ",</p>\r\n  \r\n  " +
                "<p>Chúng tôi xin thông báo hóa đơn tiền " + Type + " tháng " + Date.AddMonths(-1) + ".</p>\r\n  \r\n  " +
                "<p>Thông tin hóa đơn:</p>\r\n  <ul>\r\n    " +
                "<li><strong>Số hóa đơn:</strong> " + Id + " </li>\r\n    " +
                "<li><strong>Loại:</strong> Tiền " + Type + " </li>\r\n   " +
                "<li><strong>Họ và tên:</strong>" + Name + " </li>\r\n   " +
                " <li><strong>Phòng:</strong> " + RoomName + "</li>\r\n    " +
                "<li><strong>Từ ngày:</strong> " + DatePre + " <strong>Đến ngày:</strong> " + Date + " </li>\r\n    " +
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
                if (x.EndAmount==0)
                {
                    x.EndAmount = int.MaxValue;
                }
                if (remainingConsumption <= 0)
                    break;

                int tierConsumption = Math.Min(remainingConsumption, x.EndAmount - x.StartAmount + 1);
                total += tierConsumption * x.Price;
                remainingConsumption -= tierConsumption;
            }

            if (remainingConsumption > 0)
            {
                // Lấy giá của mức cao nhất
                var highestRate = rates.OrderByDescending(x => x.Tier).FirstOrDefault();
                if (highestRate != null)
                {
                    total += remainingConsumption * highestRate.Price;
                }
            }

            return total;
        }
    }
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DatePayment { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set;}
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }

    }

    public class RoomInvoiceModel
    {
        public int Id { get; set; }
        public int pageindex { get; set; }
        public int pagesize { get; set; }
        public int Status { get; set; }
        public decimal? Low { get; set; }
        public decimal? High { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string Type { get; set; }
        public string Filter { get; set; }
        public int BuildingId { get; set; }
        public int CampusId { get; set; }
    }
}

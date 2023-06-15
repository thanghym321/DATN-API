using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.Application.User;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace DATN.Application.BLL
{
    public class ManageRoomRegistration : IManageRoomRegistration
    {
        private readonly DATN_CFContext _context;
        private readonly ISendMailService _sendMailService;
        private readonly IMemoryCache _cache;

        public ManageRoomRegistration(DATN_CFContext context, ISendMailService sendMailService, IMemoryCache cache)
        {
            _context = context;
            _sendMailService = sendMailService;
            _cache = cache;
        }

        //private Timer _timer;

        //public void Start()
        //{

        //    _timer = new Timer(async (_) =>
        //    {
        //        await Get();
        //    }, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        //}
        public async Task<List<RoomRegistrationViewModel>> Get()
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        select new { a, b, c };
            return await query.Select(x => new RoomRegistrationViewModel()
            {
                Id = x.a.Id,
                UserId = x.b.Id,
                RoomName = x.c.Name,
                Name = x.b.Name,
                Avatar = x.b.Avatar,
                Email = x.b.Email,
                Phone = x.b.Phone,
                DateRegistration = x.a.DateRegistration,
                Status = x.a.Status,

            }).ToListAsync();
        }
        public async Task<RoomRegistrationViewModel> GetById(int Id)
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id

                        select new RoomRegistrationViewModel
                        {
                            Id = a.Id,
                            UserId = b.Id,
                            RoomName = c.Name,
                            Name = b.Name,
                            Avatar = b.Avatar,
                            Email = b.Email,
                            Phone = b.Phone,
                            DateRegistration = a.DateRegistration,
                            Status = a.Status,
                        };

            var roomregistration = await query.FirstOrDefaultAsync(x => x.Id == Id);

            return roomregistration;
        }
        public async Task<PageResult<RoomRegistrationViewModel>> GetAllPaging(RoomRegistrationModel request)
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        join e in _context.Buildings on c.BuildingId equals e.Id
                        join f in _context.Campuses on e.CampusId equals f.Id
                        select new { a , b , c , e , f };

            var student = _context.Accounts.Where(x => x.UserId == request.Id).FirstOrDefault();

            if (student.Role == (int)Role.Student)
            {
                query = query.Where(x => x.a.UserId == request.Id);
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(x => x.b.Name.ToLower().Contains(request.Name.ToLower()));
            }
            if (request.Month.HasValue)
            {
                query = query.Where(x => x.a.DateRegistration.Month == request.Month);
            }
            if (request.Year.HasValue)
            {
                query = query.Where(x => x.a.DateRegistration.Year == request.Year);
            }
            if (request.Status != -1)
            {
                query = query.Where(x => x.a.Status == request.Status);
            }
            if (request.Filter == "")
            {
                query = query.OrderByDescending(x => x.a.DateRegistration);

            }
            if (request.Filter == "new")
            {
                query = query.OrderByDescending(x => x.a.DateRegistration);

            }
            if (request.Filter == "old")
            {
                query = query.OrderBy(x => x.a.DateRegistration);

            }
            if (request.BuildingId != -1)
            {
                query = query.Where(x => x.e.Id == request.BuildingId);
            }
            if (request.CampusId != -1)
            {
                query = query.Where(x => x.f.Id == request.CampusId);
            }

            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageindex - 1) * request.pagesize).Take(request.pagesize)
            .Select(x => new RoomRegistrationViewModel()
            {
                Id = x.a.Id,
                UserId = x.b.Id,
                RoomName = x.c.Name,
                Name = x.b.Name,
                Avatar = x.b.Avatar,
                Email = x.b.Email,
                Phone = x.b.Phone,
                DateRegistration = x.a.DateRegistration,
                Status = x.a.Status,

            }).ToListAsync();

            var pageResult = new PageResult<RoomRegistrationViewModel>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;
        }
        public async Task<int> Create(RoomRegistrationModel request)
        {
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if(room.Status==(int)StatusRoom.full)
            {
                return 0;
            }

            var query = _context.RoomRegistrations;
            //Kiểm tra xem người đăng ký đã đăng ký phòng khác hay ở trong phòng chưa 
            foreach (var item in query)
            {
                if (item.UserId == request.UserId && item.Status!=(int)StatusReg.leave)
                {
                    return 2;

                }
            }
            var roomregistration = new RoomRegistration()
            {
                RoomId = request.RoomId,
                UserId = request.UserId,
            };
            _context.RoomRegistrations.Add(roomregistration);
            await _context.SaveChangesAsync();       

            return 1;
        }
        public async Task<int> Confirm(int Id)
        {
            //chuyển trạng thái đăng ký thành đã xác nhận
            var roomregistration = await _context.RoomRegistrations.FindAsync(Id);
            roomregistration.Status = (int)StatusReg.confirm;
            await _context.SaveChangesAsync();

            //chuyển trạng thái phòng thành đã đầy khi đủ số sinh viên
            var room = await _context.Rooms.FindAsync(roomregistration.RoomId);
            int totaluser = await _context.RoomRegistrations.Where(x =>
            x.RoomId == roomregistration.RoomId
            && x.Status == (int)StatusReg.confirm
            ).CountAsync();

            if (totaluser == room.Bed)
            {
                room.Status = (int)StatusRoom.full;
            }

            var obj =await GetById(Id);        
            var email = obj.Email;
            var subject = "Thông báo đăng ký thành công";
            var htmlMessage = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  " +
                "<title>Thông báo đăng ký phòng ký túc xá</title>\r\n</head>\r\n<body>\r\n  " +
                "<h1>Thông báo đăng ký phòng ký túc xá</h1>\r\n  " +
                "<p>Xin chào " + obj.Name + ",</p>\r\n  \r\n  " +
                "<p>Chúng tôi xin thông báo rằng yêu cầu đăng ký phòng ký túc xá của bạn đã được xác nhận.</p>\r\n  \r\n  " +
                "<p>Thông tin đăng ký:</p>\r\n  <ul>\r\n    " +
                "<li><strong>Họ và tên:</strong>" + obj.Name + " </li>\r\n   " +
                " <li><strong>Email:</strong> " + obj.Email + "</li>\r\n    " +
                "<li><strong>Số điện thoại:</strong> " + obj.Phone + " </li>\r\n    " +
                "<li><strong>Phòng ký túc xá:</strong> " + obj.RoomName + " </li>\r\n    " +
                "<p>Cám ơn bạn đã đăng ký phòng ký túc xá. Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi.</p>\r\n  \r\n  " +
                "<p>Trân trọng,</p>\r\n  KTX UTEHY </p>\r\n</body>\r\n</html>";

            await _sendMailService.SendEmailAsync(email, subject, htmlMessage);

            return 1;
        }
        public async Task<int> SendMailLeave(string Email, int Id)
        {
            try
            {
                string confirmationCode = Guid.NewGuid().ToString("N").Substring(0, 5);

                var email = Email;
                var subject = "Xác nhận trả phòng";
                var htmlMessage = $"Nhấn vào đường link sau để xác nhận: http://localhost:5000/api/RoomRegistration/VerifyLeave?Id={Id}&Code={confirmationCode}";

                await _sendMailService.SendEmailAsync(email, subject, htmlMessage);

                _cache.Set(Id, confirmationCode, TimeSpan.FromMinutes(5));

                return 1;
            }
            catch (Exception) { return 2; }
        }
        public async Task<int> VerifyLeave(string Code,int Id)
        {
            if (_cache.TryGetValue(Id, out string cachedVerificationCode))
            {
                // So sánh mã xác nhận nhập vào với mã xác nhận lưu trong cache
                if (Code == cachedVerificationCode)
                {
                    var invoice = await _context.Invoices.Where(x => x.UserId == Id && x.Status == (int)StatusInvoice.unpaid).CountAsync();

                    if (invoice > 0)
                    {
                        return 2;
                    }

                    var roomRegistration = await _context.RoomRegistrations.FirstOrDefaultAsync(x => x.UserId == Id && x.Status == (int)StatusReg.confirm);
                    roomRegistration.Status = (int)StatusReg.leave;

                    await _context.SaveChangesAsync();

                    // Xóa mã xác nhận khỏi cache
                    _cache.Remove(Id);
                    return 1;
                }
            }
            return 3;
           
        }
    }
    public class RoomRegistrationModel
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public int pageindex { get; set; }
        public int pagesize { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string Filter { get; set; }
        public int BuildingId { get; set; }
        public int CampusId { get; set; }
    }
    public class RoomRegistrationViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RoomName { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateRegistration { get; set; }
        public int Status { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
    }
}

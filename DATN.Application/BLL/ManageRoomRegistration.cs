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
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DATN.Application.BLL
{
    public class ManageRoomRegistration : IManageRoomRegistration
    {
        private readonly DATN_CFContext _context;
        private readonly ISendMailService _sendMailService;
        public ManageRoomRegistration(DATN_CFContext context, ISendMailService sendMailService)
        {
            _context = context;
            _sendMailService = sendMailService;
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
        public async Task<PageResult<RoomRegistrationViewModel>> GetAllPaging(int pageindex, int pagesize, int Status)
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        select new { a, b, c };

            if (Status != -1)
            {
                query=query.Where(x => x.a.Status == Status);
            }

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
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

            var roomregistration = await query.SingleOrDefaultAsync(x => x.Id == Id);

            return roomregistration;
        }
        public async Task<List<RoomRegistrationViewModel>> GetByIdUser(int Id)
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        select new { a , b , c };

              return await query.Where(x => x.a.UserId==Id)
              .OrderByDescending(x => x.a.DateRegistration)
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

        //public async Task<int> Delete(int Id)
        //{
        //    var room = await _context.Rooms.FindAsync(Id);

        //    _context.Rooms.Remove(room);
        //    await _context.SaveChangesAsync();

        //    return 1;
        //}
    }
    public class RoomRegistrationModel
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
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
    }
}

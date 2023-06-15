using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL
{
    public class ManageRoom : IManageRoom
    {
        private readonly DATN_CFContext _context;
        public ManageRoom(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<RoomViewModel>> Get(string Name)
        {
            var query = from a in _context.Rooms
                        join b in _context.Buildings on a.BuildingId equals b.Id
                        select new { a , b };

            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(x => x.a.Name.ToLower().Contains(Name.ToLower()));
            }

            return await query.Select(x => new RoomViewModel()
            {
                Id = x.a.Id,
                BuildingId = x.a.BuildingId,
                BuildingName = x.b.Name,
                Name = x.a.Name,
                Type = x.a.Type,
                Bed = x.a.Bed,
                Price = x.a.Price,
                Status = x.a.Status,
            }
           ).ToListAsync();
        }
        public async Task<RoomByIdViewModel> GetRoomByUserId(int Id)
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        select new { a, b , c };

            var room = await query.FirstOrDefaultAsync(x => x.a.UserId == Id && x.a.Status == (int)StatusReg.confirm);

            var TotalUser = await query.CountAsync(x => x.a.RoomId==room.c.Id && x.a.Status == (int)StatusReg.confirm);

            var obj = new RoomByIdViewModel()
            {
                Id = room.c.Id,
                Name = room.c.Name,
                Type = room.c.Type,
                Bed = room.c.Bed,
                Price = room.c.Price,
                Status = room.c.Status,
                TotalUser = TotalUser
            };

            return obj;

        }
        public async Task<PageResult<Users>> GetAllPagingUserInRoom(int pageindex, int pagesize, int Id)
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        select new { a, b, c };

            var room = await query.FirstOrDefaultAsync(x => x.a.UserId == Id && x.a.Status == (int)StatusReg.confirm);

            var User = query.Where(x => x.a.RoomId == room.c.Id && x.a.Status == (int)StatusReg.confirm);

            int totalRow = await User.CountAsync();
            var data = await User.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new Users()
            {
                Id = x.a.Id,
                Name = x.b.Name,
                DateOfBirth = x.b.DateOfBirth,
                Gender = x.b.Gender,
                Avatar = x.b.Avatar,
                Address = x.b.Address,
                Email = x.b.Email,
                Phone = x.b.Phone,

            }).ToListAsync();

            var pageResult = new PageResult<Users>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<PageResult<RoomViewModel>> GetAllPaging(RoomModel request)
        {
            var query = from a in _context.RoomRegistrations
                        join b in _context.Users on a.UserId equals b.Id
                        join c in _context.Rooms on a.RoomId equals c.Id
                        join d in _context.Buildings on c.BuildingId equals d.Id
                        join e in _context.Campuses on d.CampusId equals e.Id

                        select new { a, b, c , d , e };

            if (request.Low.HasValue && request.High.HasValue)
            {
                query = query.Where(x => x.c.Price >= request.Low && x.c.Price <= request.High);
            }
            if (request.Status != -1)
            {
                query = query.Where(x => x.c.Status == request.Status);
            }
            if (request.BuildingId != -1)
            {
                query = query.Where(x => x.d.Id == request.BuildingId);
            }
            if (request.CampusId != -1)
            {
                query = query.Where(x => x.e.Id == request.CampusId);
            }


            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageindex - 1) * request.pagesize).Take(request.pagesize)
            .Select(x => new RoomViewModel()
            {
                Id = x.a.Id,
                BuildingId = x.c.BuildingId,
                BuildingName = x.d.Name,
                Name = x.c.Name,
                Type = x.c.Type,
                Bed = x.c.Bed,
                Price = x.c.Price,
                Status = x.c.Status,

            }).ToListAsync();

            var pageResult = new PageResult<RoomViewModel>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<RoomViewModel> GetById(int Id)
        {
            var query = from a in _context.Rooms
                        join b in _context.Buildings on a.BuildingId equals b.Id
                        select new RoomViewModel
                        {
                            Id = a.Id,
                            BuildingId = a.BuildingId,
                            BuildingName = b.Name,
                            Name = a.Name,
                            Type = a.Type,
                            Bed = a.Bed,
                            Price = a.Price,
                            Status = a.Status,
                        };

            var obj = await query.SingleOrDefaultAsync(x => x.Id == Id);

            return obj;
        }
        public async Task<int> Create(Room request)
        {
            var obj = new Room()
            {
                BuildingId = request.BuildingId,
                Name = request.Name,
                Type = request.Type,
                Bed = request.Bed,
                Price = request.Price,
            };

            _context.Rooms.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(Room request)
        {
            var obj = await _context.Rooms.FindAsync(request.Id);

            obj.BuildingId = request.BuildingId;
            obj.Name = request.Name;
            obj.Type = request.Type;
            obj.Bed = request.Bed;
            obj.Price = request.Price;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.Rooms.FindAsync(Id);

            _context.Rooms.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
    public class RoomViewModel : Room
    {
        public string BuildingName { get; set; }
    }
    public class RoomByIdViewModel : Room
    {
        public int TotalUser { get; set; }
    }
    public class RoomModel
    {
        public int Id { get; set; }
        public int pageindex { get; set; }
        public int pagesize { get; set; }
        public int Status { get; set; }
        public decimal? Low { get; set; }
        public decimal? High { get; set; }
        public string Filter { get; set; }
        public int BuildingId { get; set; }
        public int CampusId { get; set; }
    }
}

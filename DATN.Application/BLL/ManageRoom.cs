using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<PageResult<RoomViewModel>> GetAllPaging(int pageindex, int pagesize, int? Status)
        {
            var query = from a in _context.Rooms
                        join b in _context.Buildings on a.BuildingId equals b.Id
                        select new { a, b };

            if (Status.HasValue)
            {
                query = query.Where(x => x.a.Status==Status);
            }

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new RoomViewModel()
            {
                Id = x.a.Id,
                BuildingId = x.a.BuildingId,
                BuildingName = x.b.Name,
                Name = x.a.Name,
                Type = x.a.Type,
                Bed = x.a.Bed,
                Price = x.a.Price,
                Status = x.a.Status,

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
}

using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DATN.Application.BLL
{
    public class ManageBuilding : IManageBuilding
    {
        private readonly DATN_CFContext _context;
        public ManageBuilding(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<BuildingViewModel>> Get()
        {
            var query = from a in _context.Buildings
                        join b in _context.Campuses on a.CampusId equals b.Id
                        select new { a , b };
            return await query.Select(x => new BuildingViewModel()
            {
                Id = x.a.Id,
                CampusId = x.a.CampusId,
                CampusName = x.b.Name,
                Name = x.a.Name,
                Floor = x.a.Floor,
                Room = x.a.Room,

            }).ToListAsync();
        }
        public async Task<PageResult<BuildingViewModel>> GetAllPaging(int pageindex, int pagesize, string Name)
        {
            var query = from a in _context.Buildings
                        join b in _context.Campuses on a.CampusId equals b.Id
                        select new { a, b };

            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(x => x.a.Name.ToLower().Contains(Name.ToLower()));
            }

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new BuildingViewModel()
            {
                Id = x.a.Id,
                CampusId = x.a.CampusId,
                CampusName = x.b.Name,
                Name = x.a.Name,
                Floor = x.a.Floor,
                Room = x.a.Room,

            }).ToListAsync();

            var pageResult = new PageResult<BuildingViewModel>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<BuildingViewModel> GetById(int Id)
        {
            var query = from a in _context.Buildings
                        join b in _context.Campuses on a.CampusId equals b.Id
                        select new BuildingViewModel
                        {
                            Id = a.Id,
                            CampusId = a.CampusId,
                            CampusName = b.Name,
                            Name = a.Name,
                            Floor = a.Floor,
                            Room = a.Room,
                        };

            var obj = await query.SingleOrDefaultAsync(x => x.Id == Id);

            return obj;
        }
        public async Task<int> Create(Building request)
        {
            var obj = new Building()
            {
                CampusId = request.CampusId,
                Name = request.Name,
                Floor = request.Floor,
                Room = request.Room,
            };

            _context.Buildings.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(Building request)
        {
            var obj = await _context.Buildings.FindAsync(request.Id);

            obj.CampusId = request.CampusId;
            obj.Name = request.Name;
            obj.Floor = request.Floor;
            obj.Room = request.Room;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.Buildings.FindAsync(Id);

            _context.Buildings.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }       
    }
    public class BuildingViewModel : Building
    {
        public string CampusName { get; set; }
    }
}

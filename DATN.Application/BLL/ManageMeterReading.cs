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
    public class ManageMeterReading : IManageMeterReading
    {
        private readonly DATN_CFContext _context;
        public ManageMeterReading(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<MeterReadingViewModel>> Get()
        {
            var query = from a in _context.MeterReadings
                        join b in _context.Rooms on a.RoomId equals b.Id
                        select new { a, b };
            return await query.Select(x => new MeterReadingViewModel()
            {
                Id = x.a.Id,
                RoomId = x.a.RoomId,
                RoomName = x.b.Name,
                ElectricityIndex = x.a.ElectricityIndex,
                WaterIndex = x.a.WaterIndex,
                Date = x.a.Date
            }
           ).ToListAsync();
        }
        public async Task<PageResult<MeterReadingViewModel>> GetAllPaging(int pageindex, int pagesize)
        {
            var query = from a in _context.MeterReadings
                        join b in _context.Rooms on a.RoomId equals b.Id
                        select new { a, b };

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new MeterReadingViewModel()
            {
                Id = x.a.Id,
                RoomId = x.a.RoomId,
                RoomName = x.b.Name,
                ElectricityIndex = x.a.ElectricityIndex,
                WaterIndex = x.a.WaterIndex,
                Date = x.a.Date

            }).ToListAsync();

            var pageResult = new PageResult<MeterReadingViewModel>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<MeterReadingViewModel> GetById(int Id)
        {
            var query = from a in _context.MeterReadings
                        join b in _context.Rooms on a.RoomId equals b.Id
                        select new MeterReadingViewModel
                        {
                            Id = a.Id,
                            RoomId = a.RoomId,
                            RoomName = b.Name,
                            ElectricityIndex = a.ElectricityIndex,
                            WaterIndex = a.WaterIndex,
                            Date = a.Date
                        };

            var obj = await query.SingleOrDefaultAsync(x => x.Id == Id);

            return obj;
        }
        public async Task<int> Create(MeterReading request)
        {
            bool isyes = _context.MeterReadings
                .Any(x => x.RoomId == request.RoomId 
                && x.Date.Month == DateTime.Now.Month 
                && x.Date.Year == DateTime.Now.Year);
            if (isyes)
            {
                return 2;
            }

            var obj = new MeterReading()
            {
                RoomId = request.RoomId,
                ElectricityIndex = request.ElectricityIndex,
                WaterIndex = request.WaterIndex,
            };

                _context.MeterReadings.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(MeterReading request)
        {
            var obj = await _context.MeterReadings.FindAsync(request.Id);

            obj.RoomId = request.RoomId;
            obj.ElectricityIndex = request.ElectricityIndex;
            obj.WaterIndex = request.WaterIndex;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.MeterReadings.FindAsync(Id);

            _context.MeterReadings.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
    public class MeterReadingViewModel : MeterReading
    {
        public string RoomName { get; set; }
    }
}

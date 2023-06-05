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
    public class ManageElectricityWaterRate : IManageElectricityWaterRate
    {
        private readonly DATN_CFContext _context;
        public ManageElectricityWaterRate(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<ElectricityWaterRate>> Get()
        {
            var query = from a in _context.ElectricityWaterRates
                        select new { a };
            return await query.Select(x => x.a).ToListAsync();
        }
        public async Task<PageResult<ElectricityWaterRate>> GetAllPaging(int pageindex, int pagesize)
        {
            var query = from a in _context.ElectricityWaterRates
                        select new { a };

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new ElectricityWaterRate()
            {
                Id = x.a.Id,
                Type = x.a.Type,
                Tier = x.a.Tier,
                StartAmount = x.a.StartAmount,
                EndAmount = x.a.EndAmount,
                Unit = x.a.Unit,
                Price = x.a.Price

            }).ToListAsync();

            var pageResult = new PageResult<ElectricityWaterRate>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<ElectricityWaterRate> GetById(int Id)
        {
            var obj = await _context.ElectricityWaterRates.FindAsync(Id);

            return obj;
        }
        public async Task<int> Create(ElectricityWaterRate request)
        {
            var obj = new ElectricityWaterRate()
            {
                Type = request.Type,
                Tier = request.Tier,
                StartAmount = request.StartAmount,
                EndAmount = request.EndAmount,
                Unit = request.Unit,
                Price = request.Price,
            };

            _context.ElectricityWaterRates.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(ElectricityWaterRate request)
        {
            var obj = await _context.ElectricityWaterRates.FindAsync(request.Id);

            obj.Type = request.Type;
            obj.Tier = request.Tier;
            obj.StartAmount = request.StartAmount;
            obj.EndAmount = request.EndAmount;
            obj.Unit = request.Unit;
            obj.Price = request.Price;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.ElectricityWaterRates.FindAsync(Id);

            _context.ElectricityWaterRates.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}

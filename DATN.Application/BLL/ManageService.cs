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
    public class ManageService : IManageService
    {
        private readonly DATN_CFContext _context;
        public ManageService(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> Get()
        {
            var query = from a in _context.Services
                        select new { a};
            return await query.Select(x => new Service()
            {
                Id = x.a.Id,
                Name = x.a.Name,
                Description = x.a.Description,
                Unit = x.a.Unit,
                Price = x.a.Price,
            }
           ).ToListAsync();
        }
        public async Task<PageResult<Service>> GetAllPaging(int pageindex, int pagesize)
        {
            var query = from a in _context.Services
                        select new { a };

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new Service()
            {
                Id = x.a.Id,
                Name = x.a.Name,
                Description = x.a.Description,
                Unit = x.a.Unit,
                Price = x.a.Price,

            }).ToListAsync();

            var pageResult = new PageResult<Service>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<Service> GetById(int Id)
        {
            var query = from a in _context.Services
                        select new Service
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            Unit = a.Unit,
                            Price = a.Price,
                        };

            var obj = await query.SingleOrDefaultAsync(x => x.Id == Id);

            return obj;
        }
        public async Task<int> Create(Service request)
        {
            var obj = new Service()
            {
                Name = request.Name,
                Description = request.Description,
                Unit = request.Unit,
                Price = request.Price,
            };

            _context.Services.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(Service request)
        {
            var obj = await _context.Services.FindAsync(request.Id);

            obj.Name = request.Name;
            obj.Description = request.Description;
            obj.Unit = request.Unit;
            obj.Price = request.Price;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.Services.FindAsync(Id);

            _context.Services.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}

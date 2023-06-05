using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DATN.Application.BLL
{
    public class ManageCampus : IManageCampus
    {
        private readonly DATN_CFContext _context;
        public ManageCampus(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<Campus>> Get()
        {
            var query = from a in _context.Campuses
                        select new { a };
            return await query.Select(x => x.a).ToListAsync();
        }
        public async Task<PageResult<Campus>> GetAllPaging(int pageindex, int pagesize, string Name)
        {
            var query = from a in _context.Campuses
                        select new { a };

            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(x => x.a.Name.ToLower().Contains(Name.ToLower()));
            }

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new Campus()
            {
                Id = x.a.Id,
                Name = x.a.Name,
                Address = x.a.Address,
                Email = x.a.Email,
                Phone = x.a.Phone,

            }).ToListAsync();

            var pageResult = new PageResult<Campus>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<Campus> GetById(int Id)
        {
            var obj = await _context.Campuses.FindAsync(Id);

            return obj;
        }
        public async Task<int> Create(Campus request)
        {
            var obj = new Campus()
            {
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
            };

            _context.Campuses.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(Campus request)
        {
            var obj = await _context.Campuses.FindAsync(request.Id);

            obj.Name = request.Name;
            obj.Address = request.Address;
            obj.Email = request.Email;
            obj.Phone = request.Phone;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.Campuses.FindAsync(Id);

            _context.Campuses.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}

using DATN.Application.BLL.Interface;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IManageReport _manageReport;
        public ReportController(IManageReport manageReport)
        {
            _manageReport = manageReport;
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            var result = await _manageReport.Get();
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] int pageindex, int pagesize)
        {
            var result = await _manageReport.GetAllPaging(pageindex, pagesize);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getbyid(int Id)
        {
            var result = await _manageReport.GetById(Id);
            if (result == null)
            {
                return BadRequest("Cannot find");
            }
            return Ok(result);

        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] Report request)
        {
            var result = await _manageReport.Create(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Create Failed");

        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] Report request)
        {
            var result = await _manageReport.Update(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Update Failed");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            var result = await _manageReport.Delete(Id);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Delete Failed");
        }
    }
}

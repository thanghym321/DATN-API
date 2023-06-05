using DATN.Application.BLL.Interface;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IManageMeterReading _manageMeterReading;
        public MeterReadingController(IManageMeterReading manageMeterReading)
        {
            _manageMeterReading = manageMeterReading;
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            var result = await _manageMeterReading.Get();
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] int pageindex, int pagesize)
        {
            var result = await _manageMeterReading.GetAllPaging(pageindex, pagesize);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getbyid(int Id)
        {
            var result = await _manageMeterReading.GetById(Id);
            if (result == null)
            {
                return BadRequest("Cannot find");
            }
            return Ok(result);

        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] MeterReading request)
        {
            var result = await _manageMeterReading.Create(request);
            if (result == 1)
            {
                return Ok(1);
            }
            if (result == 2)
            {
                return Ok(2);
            }

            return BadRequest("Create Failed");

        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] MeterReading request)
        {
            var result = await _manageMeterReading.Update(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Update Failed");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            var result = await _manageMeterReading.Delete(Id);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Delete Failed");
        }
    }
}

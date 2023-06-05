using DATN.Application.BLL.Interface;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IManageBuilding _manageBuilding;
        public BuildingController(IManageBuilding manageBuilding)
        {
            _manageBuilding = manageBuilding;
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            var result = await _manageBuilding.Get();
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] int pageindex, int pagesize, string Name)
        {
            var result = await _manageBuilding.GetAllPaging(pageindex, pagesize, Name);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getbyid(int Id)
        {
            var result = await _manageBuilding.GetById(Id);
            if (result == null)
            {
                return BadRequest("Cannot find");
            }
            return Ok(result);

        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] Building request)
        {
            var result = await _manageBuilding.Create(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Create Failed");

        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] Building request)
        {
            var result = await _manageBuilding.Update(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Update Failed");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            var result = await _manageBuilding.Delete(Id);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Delete Failed");
        }
    }
}

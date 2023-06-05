using DATN.Application.BLL;
using DATN.Application.BLL.Interface;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomRegistrationController : ControllerBase
    {
        private readonly IManageRoomRegistration _manageRoomRegistration;
        public RoomRegistrationController(IManageRoomRegistration manageRoomRegistration)
        {
            _manageRoomRegistration = manageRoomRegistration;
        }
        //[HttpGet]
        //public async Task<IActionResult> get()
        //{
        //    var result = await _manageRoomRegistration.Get();
        //    if (result == null)
        //    {
        //        return BadRequest("Get Failed");
        //    }

        //    return Ok(result);
        //}

        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] int pageindex, int pagesize, int Status)
        {
            var result = await _manageRoomRegistration.GetAllPaging(pageindex, pagesize, Status);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        //[HttpGet("{Id}")]
        //public async Task<IActionResult> getbyid(int Id)
        //{
        //    var result = await _manageRoomRegistration.GetById(Id);
        //    if (result == null)
        //    {
        //        return BadRequest("Cannot find");
        //    }
        //    return Ok(result);

        //}
        [HttpGet("{Id}")]
        public async Task<IActionResult> getbyiduser(int Id)
        {
            var result = await _manageRoomRegistration.GetByIdUser(Id);
            if (result == null)
            {
                return BadRequest("Cannot find");
            }
            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] RoomRegistrationModel request)
        {
            var result = await _manageRoomRegistration.Create(request);
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
        public async Task<IActionResult> confirm([FromBody] int Id)
        {
            var result = await _manageRoomRegistration.Confirm(Id);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Confirm Failed");

        }
    }
}

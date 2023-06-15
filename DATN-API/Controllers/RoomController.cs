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
    public class RoomController : ControllerBase
    {
        private readonly IManageRoom _manageRoom;
        public RoomController(IManageRoom manageRoom)
        {
            _manageRoom = manageRoom;
        }

        [HttpGet]
        public async Task<IActionResult> get(string Name)
        {
            var result = await _manageRoom.Get(Name);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetRoomByUserId(int Id)
        {
            var result = await _manageRoom.GetRoomByUserId(Id);
            if (result == null)
            {
                return BadRequest("Cannot find");
            }
            return Ok(result);

        }


        [HttpGet]
        public async Task<IActionResult> GetAllPagingUserInRoom([FromQuery] int pageindex, int pagesize, int Id)
        {
            var result = await _manageRoom.GetAllPagingUserInRoom(pageindex, pagesize, Id);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] RoomModel request)
        {
            var result = await _manageRoom.GetAllPaging(request);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getbyid(int Id)
        {
            var result = await _manageRoom.GetById(Id);
            if (result == null)
            {
                return BadRequest("Cannot find");
            }
            return Ok(result);

        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] Room request)
        {
            var result = await _manageRoom.Create(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Create Failed");

        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] Room request)
        {
            var result = await _manageRoom.Update(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Update Failed");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            var result = await _manageRoom.Delete(Id);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Delete Failed");
        }
    }
}

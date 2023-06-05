using DATN.Application.BLL.Interface;
using DATN.Application.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private IManageUser _manageUser;

        public UserController(IManageUser manageUser)
        {
            _manageUser = manageUser;

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult authenticate([FromBody] AuthenticateModel model)
        {
            var result = _manageUser.Authenticate(model.username, model.password);

            if (result == null)
                return BadRequest(new { message = "Tài khoản hoặc mật khẩu sai!" });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            var result = await _manageUser.Get();
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] int pageindex, int pagesize, string UserName, string Name, int Role)
        {
            var result = await _manageUser.GetAllPaging(pageindex, pagesize, UserName, Name, Role);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getbyid(int Id)
        {
            var result = await _manageUser.GetById(Id);
            if (result == null)
            {
                return BadRequest("Cannot find user");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] UserModel request)
        {
            var result = await _manageUser.Create(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });

            }

            return BadRequest("Create Failed");

        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] UserModel request)
        {

            var result = await _manageUser.Update(request);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Update Failed");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> delete(int Id)
        {

            var result = await _manageUser.Delete(Id);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }
            return BadRequest("Delete Failed");
        }
    }
}

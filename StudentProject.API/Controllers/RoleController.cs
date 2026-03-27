using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Interface;
using Student.Model;

namespace StudentProject.API.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IRole _role;
        public RoleController(IRole role, Serilog.ILogger logger)
        {
            _role = role;
            _logger = logger;
        }


        [HttpGet("GetAll")]
        public ListRoleResponse GetAll()
        {
            return _role.GetAll();
        }


        [HttpGet("GetById")]
        public RoleResponse GetById(int Id)
        {

            return _role.GetById(Id);
        }


        [HttpPost("Add")]
        public RoleResponse Insert(RoleModel param)
        {

            return _role.Insert(param);


        }


        [HttpPut("Update")]
        public RoleResponse Update(RoleModel param)
        {
            return _role.Update(param);


        }


        [HttpDelete("Delete")]
        public RoleResponse Delete(int id)
        {
            return _role.Delete(id);


        }
    }
}

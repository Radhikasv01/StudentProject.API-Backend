using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Student.Interface;
using Student.Model;
using Student.Service;

namespace StudentProject.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _reg;
        public LoginController(ILogin registerService)
        {
            _reg = registerService;
        }

        [HttpPost("Login")]
        public ResponseToken Authreg(LoginModel loginuser)
        {
            return _reg.Authreg(loginuser);
        }
    }
}

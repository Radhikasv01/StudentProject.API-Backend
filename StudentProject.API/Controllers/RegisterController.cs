using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Interface;
using Student.Model;
using System.Net.Mime;

namespace StudentProject.API.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [EnableCors("AllowAllOrigins")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IRegister _service;
        public RegisterController(IRegister service, Serilog.ILogger logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpGet("GetAll")]
        public ListRegisterResponse GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return _service.GetAll(pageNumber, pageSize);
        }


        [HttpGet("GetById")]
        public RegisterResponse GetById(int Id)
        {

            return _service.GetById(Id);
        }


        [HttpPost("Add")]
        public RegisterResponse Insert(RegisterModel param)
        {

            return _service.Insert(param);


        }


        [HttpPut("Update")]
        public RegisterResponse Update(RegisterModel param)
        {
            return _service.Update(param);


        }


        [HttpDelete("Delete")]
        public RegisterResponse Delete(int id)
        {
            return _service.Delete(id);


        }
        //[HttpGet("ExportToExcel")]
        //public IActionResult ExportToExcel()
        //{
        //    var fileContents = _service.ExportToExcel();
        //    var fileName = "Registrations.xlsx";
        //    var contentType = MediaTypeNames.Application.Octet; // Default content type

        //    return File(fileContents, contentType, fileName);
        //}
    }
}

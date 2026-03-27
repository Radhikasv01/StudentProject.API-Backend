using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Interface;
using Student.Model;
using System.Drawing.Printing;

namespace StudentProject.API.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [EnableCors("AllowAllOrigins")]
    [ApiController]
    public class StudentBookController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IStudentBooks _service;
        public StudentBookController(IStudentBooks service, Serilog.ILogger logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("GetAll")]
        public ListStudentBooksResponse GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return _service.GetAll( pageNumber, pageSize);
        }
        [HttpGet("GetById")]
        public StudentBooksResponse GetById(int Id)
        {

            return _service.GetById(Id);
        }
        [HttpPost("Add")]
        public StudentBooksResponse Insert(StudentBooksModel param)
        {

            return _service.Insert(param);


        }
        [HttpPut("Update")]
        public StudentBooksResponse Update(StudentBooksModel param)
        {
            return _service.Update(param);


        }
        [HttpDelete("Delete")]
        public StudentBooksResponse Delete(int id)
        {
            return _service.Delete(id);


        }
        [HttpGet("GetByFilter")]
        public ListStudentBooksResponse GetByFilter(int Id, int pageNumber, int pageSize)
        {
            return _service.GetByFilter(Id,  pageNumber,  pageSize);
        }

    }
}

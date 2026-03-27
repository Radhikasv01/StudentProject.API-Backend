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
    public class StudentController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IStudent _student;
        public StudentController(IStudent student, Serilog.ILogger logger)
        {
            _student = student;
            _logger = logger;
        }


        [HttpGet("GetAll")]
        public ListStudentModelResponse GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return _student.GetAll( pageNumber, pageSize);
        }


        [HttpGet("GetById")]
        public StudentModelResponse GetById(int Id)
        {

            return _student.GetById(Id);
        }


        [HttpPost("Add")]
        public StudentModelResponse Insert(StudentModel param)
        {

            return _student.Insert(param);


        }


        [HttpPut("Update")]
        public StudentModelResponse Update(StudentModel param)
        {
            return _student.Update(param);


        }


        [HttpDelete("Delete")]
        public StudentModelResponse Delete(int id)
        {
            return _student.Delete(id);


        }
    }
}

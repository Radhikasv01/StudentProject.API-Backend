using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Interface;
using Student.Model;

namespace StudentProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCoarseController : ControllerBase
    {
        private readonly IStudentCoarse _coarse;
        public StudentCoarseController(IStudentCoarse coarse)
        {
            _coarse = coarse;
        }
        [HttpGet("GetAll")]
        public ListStudentCourseResponse GetAll()
        {
            return _coarse.GetAll();
        }
        [HttpPost("Insert")]
        public StudentCoarseResponse Insert(StudentCourse studentCourse) 
        {
        
            return _coarse.Insert(studentCourse);
        }
       
        [HttpPut("Update")]
        public StudentCoarseResponse Update(StudentCourse course)
        {
            return _coarse.Update(course);


        }
        [HttpGet("GetById")]
        public StudentCoarseResponse GetById(int Id)
        {

            return _coarse.GetById(Id);
        }
        [HttpDelete("Delete")]
        public StudentCoarseResponse Delete(int Id) 
        { 
            return _coarse.Delete(Id);
        
        }

    }
}

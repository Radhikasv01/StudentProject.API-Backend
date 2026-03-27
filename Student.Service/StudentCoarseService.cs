using Student.Interface;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Student.Service
{
    public class StudentCoarseService : IStudentCoarse
    {
        private readonly  StudentDbContext _studentService;
        public StudentCoarseService(StudentDbContext studentService)
        {
            _studentService = studentService;
        }

        public StudentCoarseResponse Delete(int Id)
        {
            StudentCoarseResponse response = new StudentCoarseResponse();
            response = new StudentCoarseResponse()
            {
                course = new StudentCourse(),
                response = new ResponseModel()
            };
            var students = _studentService.Coarses.FirstOrDefault(c => c.Id == Id);
            if (students!=null)
            {
                _studentService.Coarses.Remove(students);
                _studentService.SaveChanges();
                response.response.IsSuccess = true;
                response.response.Message = "In The ID :" + Id + "Data are Deleted";
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public ListStudentCourseResponse GetAll()
        {
            ListStudentCourseResponse response = new ListStudentCourseResponse();
            response = new ListStudentCourseResponse()
            {
                allcourse = new List<StudentCourse>(),
                response = new ResponseModel()
            };
            List<StudentCourse> Getall = _studentService.Coarses.ToList();
            if (Getall != null && Getall.Count > 0)
            {
                response.response.IsSuccess = true;
                response.allcourse = Getall;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records available";
            }
            return response;
        }

        public  StudentCoarseResponse GetById(int Id)
        {
            StudentCoarseResponse response = new StudentCoarseResponse();
            response = new StudentCoarseResponse()
            {
                course = new StudentCourse(),
                response = new ResponseModel()
            };
            StudentCourse GetbyId =  _studentService.Coarses.FirstOrDefault(x => x.Id == Id);
            if (GetbyId != null)
            {
                response.response.IsSuccess = true;
                response.course = GetbyId;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public StudentCoarseResponse Insert(StudentCourse course)
        {
            StudentCoarseResponse response = new StudentCoarseResponse();
            response = new StudentCoarseResponse()
            {
                course = new StudentCourse(),
                response = new ResponseModel()
            };

            var result= _studentService.Coarses.Add(course);
            if (result != null)
            {
                _studentService.SaveChanges();
                response.response.IsSuccess = true;
                response.course = course;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public StudentCoarseResponse Update(StudentCourse course)
        {
            StudentCoarseResponse response = new StudentCoarseResponse();
            response = new StudentCoarseResponse()
            {
                course = new StudentCourse(),
                response = new ResponseModel()
            };
            
            var Edit = _studentService.Coarses.Update(course);
            
            if (Edit != null)
            {
                _studentService.SaveChanges();
                response.response.IsSuccess = true;
                response.course = course;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }
    }
}

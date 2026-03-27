using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Model
{
    public class RoleModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
    public class ListRoleResponse
    {
        public ResponseModel response { get; set; }
        public List<RoleModel> studentrole { get; set; }
    }
    public class RoleResponse
    {
        public StudentBooksModel books;
        public ResponseModel response { get; set; }
        public  RoleModel role { get; set; }
    }
    public class ListRegisterResponse
    {
        public List<StudentModel> studentModels;
        public ResponseModel response { get; set; }
        public int TotalCount { get; set; }
        public List<RegisterModel> register { get; set; }
        
    }
    public class RegisterResponse
    {
        public RoleModel addrole;
        public ResponseModel response { get; set; }
        public RegisterModel register { get; set; }
    }
    public class ListStudentModelResponse
    {
        public ResponseModel response { get; set; }
        public int TotalCount { get; set; }
        public List<StudentModel> students { get; set; }
    }
    public class StudentModelResponse
    {
        public ResponseModel response { get; set; }

        public int TotalCount { get; set; }

        public StudentModel student { get; set; }
    }
    public class ListStudentBooksResponse
    {
        public ResponseModel response { get; set; }

        public int TotalCount { get; set; }
        public List<StudentBooksModel> allbooks { get; set; }
    }
    public class StudentBooksResponse
    {
        public ResponseModel response { get; set; }
        public StudentBooksModel books { get; set; }
    }
    public class ListStudentCourseResponse
    {
        public ResponseModel response { get; set; }
        public int TotalCount { get; set; }
        public List<StudentCourse> allcourse { get; set; }
    }
    public class StudentCoarseResponse
    {
        public ResponseModel response { get; set; }
        public StudentCourse course { get; set; }
    }
    public class ListSportsResponse
    {
        public ResponseModel response { get; set; }
        public int TotalCount { get; set; }
        public List<SportsModel> alls { get; set; }
    }
    public class SportsResponse
    {
        public ResponseModel response { get; set; }
        public SportsModel sports { get; set; }
    }
    public class ResponseToken
    {
        public ResponseModel response { get; set; }
        public LoginModel login { get; set; }
    }
}

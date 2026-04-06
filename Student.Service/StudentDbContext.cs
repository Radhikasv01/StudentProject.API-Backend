using Microsoft.EntityFrameworkCore;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Service
{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }
        public DbSet<StudentCourse> Coarses { get; set; }
        public DbSet<SportsModel> Sports { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<StudentBooksModel> BooksModels { get; set; }
        public DbSet<RoleModel> Roles { get; set; } 
        public DbSet<RegisterModel> RegisterModels { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }
        //public DbSet<EmailModel> EmailModels { get; set; }
       


    }

}

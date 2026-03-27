using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Model
{
    public class StudentCourse
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string Description {  get; set; }
        [Required]
        public int Credits {  get; set; }
        
        public DateTime JoiningDate { get; set; }
        public DateTime EndingDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Model
{
    public class StudentModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StudentName { get; set; }
        [EmailAddress]
        public string StudentEmail { get; set; }
        [Required]
        public string StudentDepartment {  get; set; }
       
        public DateTime StudentJoiningDate {  get; set; }
        [Required]
        public float StudentWeight { get; set; }
        [Required]
        public float StudentHeight {  get; set; }
        [Required]
        public string BloodGroup {  get; set; }
        public bool IsActive {  get; set; }




    }
}

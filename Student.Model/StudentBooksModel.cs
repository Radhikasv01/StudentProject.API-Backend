using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Model
{
    public class StudentBooksModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StudentId {  get; set; }


        [Required]
        public string BookName {  get; set; }

        [Required]
        public bool IsActive {  get; set; }
    }
}

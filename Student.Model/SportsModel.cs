using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Model
{
    public class SportsModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StudentName { get; set; }
        [Required]
        public string SportsName {  get; set; }
        [Required]
        public string SportsDescription { get; set; }
        [Required]
        public DateTime SportsTime { get; set; }
    }
}

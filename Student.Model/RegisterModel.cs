using System.ComponentModel.DataAnnotations;

namespace Student.Model
{
    public class RegisterModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Phone]
        public string Mobile { get; set; }
        public DateTime Date {  get; set; }
        [Required]
        public string Address {  get; set; }
        [Required]
        public int Pincode {  get; set; }
        [Required]
        public int RoleId {  get; set; }
        public bool IsActive { get; set; }

    }
}

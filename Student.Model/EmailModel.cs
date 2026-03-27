//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Student.Model
//{
//    public class EmailModel
//    {
//        [Key]
//        public int Id { get; set; }
//        public string From { get; set; }
//        public List<string> To { get; set; } = new List<string>();
//        public string Subject { get; set; }
//        public string Body { get; set; }
//        public List<string> Attachments { get; set; } = new List<string>(); // Paths to attachment files
//    }
//    public class ListEmailResponse
//    {
//        public List<EmailModel> Emails { get; set; }
//        public ResponseModel Response { get; set; }
//    }
//    public class EmailResponse
//    {
//        public ResponseModel Response { get; set; }
//        public EmailModel Model { get; set; }
//    }
//    public class EmailSettings
//    {
//        public string SenderEmail { get; set; }
//    }

//}

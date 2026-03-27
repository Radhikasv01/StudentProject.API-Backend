//using Npgsql;
//using Student.Interface;
//using Student.Model;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Net.Mail;
//using System.Text;
//using System.Threading.Tasks;

//namespace Student.Service
//{
//    public class EmailService:IEmail
//    {

//        private readonly IRepository<EmailModel> _repository;
//        private readonly string _Connection;
//        private readonly string _smtpServer;
//        private readonly int _smtpPort;
//        private readonly string _smtpUser;
//        private readonly string _smtpPass;
//        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass,IRepository<EmailModel> email)
//        {
//            _smtpServer = smtpServer;
//            _smtpPort = smtpPort;
//            _smtpUser = smtpUser;
//            _smtpPass = smtpPass;
//            _Connection = Settings.ConnectionString;
//        }
//        public IDbConnection Connection
//        {
//            get
//            {
//                return new NpgsqlConnection(_Connection);
//            }
//        }
//        public async Task<EmailResponse> SendEmailAsync(EmailModel email)
//        {
//            var response = new EmailResponse();
//            try
//            {
//                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
//                {
//                    smtpClient.Credentials = new System.Net.NetworkCredential(_smtpUser, _smtpPass);
//                    smtpClient.EnableSsl = true;

//                    var mailMessage = new MailMessage
//                    {
//                        From = new MailAddress(email.From),
//                        Subject = email.Subject,
//                        Body = email.Body,
//                        IsBodyHtml = true
//                    };

//                    foreach (var recipient in email.To)
//                    {
//                        mailMessage.To.Add(recipient);
//                    }

//                    if (email.Attachments != null)
//                    {
//                        foreach (var attachment in email.Attachments)
//                        {
//                            mailMessage.Attachments.Add(new Attachment(attachment));
//                        }
//                    }

//                    await smtpClient.SendMailAsync(mailMessage);
//                }

//                response.Response.IsSuccess = true;
//                response.Response.Message = "Email sent successfully.";
//            }
//            catch (Exception ex)
//            {
//                response.Response.IsSuccess = false;
//                response.Response.Message = $"Error sending email: {ex.Message}";
//                // Log the exception as needed
//            }

//            return response;
//        }

//    }
//}

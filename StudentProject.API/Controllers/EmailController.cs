//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Student.Interface;
//using Student.Model;
//using Student.Service;

//namespace StudentProject.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmailController : ControllerBase
//    {
//        private readonly IEmail _emailService;

//        public EmailController(IEmail emailService)
//        {
//            _emailService = emailService;
//        }

//        [HttpPost("send-email")]
//        public async Task<IActionResult> SendEmail([FromBody] EmailModel emailModel)
//        {
//            var response = await _emailService.SendEmailAsync(emailModel);
//            if (response.Response.IsSuccess)
//            {
//                return Ok(response.Response.IsSuccess);
//            }
//            return BadRequest(response.Response.Message);
//        }
//    }
//}

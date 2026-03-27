using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Interface;
using Student.Model;

namespace StudentProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly ISports _sportss;
        public SportsController(ISports sportss)
        {
            _sportss= sportss;
        }
        [HttpGet("GetALl")]
        public ListSportsResponse GetAll()
        {

        return _sportss.GetAll(); 
        }
        [HttpGet("GetById")]
        public SportsResponse GetById(int Id)
        {
            return _sportss.GetById(Id);
        }
        [HttpPost("Insert")]
        public SportsResponse Insert(SportsModel model)
        {
            return _sportss.Insert(model);
        }
        [HttpPut("Update")]
        public SportsResponse Update(SportsModel model)
        {
            return _sportss.Update(model);
        }
        [HttpDelete("Delete")]
        public SportsResponse Delete(int Id)
        {
            return _sportss.Delete(Id);
        }
    }
}

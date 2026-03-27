using Student.Interface;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Service
{
    public class SportsService : ISports
    {
        private readonly StudentDbContext _sports;
        public SportsService(StudentDbContext sports)
        {
            _sports = sports;
        }
        public SportsResponse Delete(int Id)
        {
            SportsResponse response = new SportsResponse();
            response = new SportsResponse()
            {
                sports = new SportsModel(),
                response=new ResponseModel()
            };
            var sports=_sports.Sports.FirstOrDefault(x => x.Id == Id);
            if(sports != null)
            {
                _sports.Sports.Remove(sports);
                _sports.SaveChanges();
                response.response.IsSuccess = true;
                response.response.Message = "Deleted Successfully";
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "Np Records Available";
            }
            return response;
            
        }

        public ListSportsResponse GetAll()
        {
            ListSportsResponse response = new ListSportsResponse();
            response = new ListSportsResponse()
            {
                alls = new List<SportsModel>(),
                response = new ResponseModel()
            };
            List<SportsModel>Getall=_sports.Sports.ToList();
            if(Getall != null&&Getall.Count>0)
            {
                response.response.IsSuccess= true;
                response.alls = Getall;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public SportsResponse GetById(int Id)
        {
            SportsResponse response = new SportsResponse();
            response = new SportsResponse()
            {
                sports = new SportsModel(),
                response = new ResponseModel()
            };
            SportsModel Getbyid=_sports.Sports.FirstOrDefault(_sports => _sports.Id == Id);
            if(Getbyid != null)
            {
                response.response.IsSuccess= true;
                response.sports = Getbyid;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public SportsResponse Insert(SportsModel sports)
        {
            SportsResponse response= new SportsResponse();
            response = new SportsResponse()
            {
                sports = new SportsModel(),
                response = new ResponseModel()
            };
            var result = _sports.Sports.Add(sports);
            if(result != null)
            {
                _sports.SaveChanges();
                response.response.IsSuccess= true;
                response.sports=sports;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public SportsResponse Update(SportsModel sports)
        {
            SportsResponse response = new SportsResponse();
            response = new SportsResponse()
            {
                sports = new SportsModel(),
                response = new ResponseModel()
            };
            var result=_sports.Sports.Update(sports);
            if(result != null)
            {
                _sports.SaveChanges();
                response.response.IsSuccess= true;
                response.sports=sports;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }
    }
}

using Student.Interface;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Service
{
    public class RoleService : IRole
    {
        private readonly string _Connection;
        private readonly IRepository<RoleModel> _repos;
        public RoleService(IRepository<RoleModel> repos)
        {
            _repos = repos;
        }

        const string get_all = @"SELECT * FROM public.""Roles""";

        const string getbyid = @"SELECT * FROM public.""Roles"" WHERE ""Id""= @Id";

        const string insert = @"INSERT INTO public.""Roles""(""Role"", ""Description"",""IsActive"") VALUES (@Role, @Description,@IsActive) RETURNING *";

        const string update = @"UPDATE public.""Roles"" SET ""Role""=@Role, ""Description""=@Description,""IsActive""=@IsActive WHERE ""Id""=@Id RETURNING *";

        const string delete = @"update  public.""Roles"" SET ""IsActive""=false WHERE ""Id""= @Id";
        public RoleResponse Delete(int Id)
        {
            RoleResponse response = new RoleResponse();
            response = new RoleResponse()
            {
                role = new RoleModel(),
                response = new ResponseModel()
            };
            var Remove = _repos.Delete(delete, new { @Id = Id });
            if (Remove)
            {
                response.response.IsSuccess = true;
                response.response.Message = "In The ID :" + Id + "Data are Deleted";
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public ListRoleResponse GetAll()
        {
            ListRoleResponse response = new ListRoleResponse();
            response = new ListRoleResponse()
            {
              studentrole= new List<RoleModel>(),
                response = new ResponseModel()
            };
            List<RoleModel> Getall = _repos.GetAllID(get_all);
            if (Getall != null && Getall.Count > 0)
            {
                response.response.IsSuccess = true;
                response.studentrole = Getall;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records available";
            }
            return response;
        }

        public RoleResponse GetById(int Id)
        {
            RoleResponse response = new RoleResponse();
            response = new RoleResponse()
            {
                role = new RoleModel(),
                response = new ResponseModel()
            };
            RoleModel GetbyId = _repos.GetById(getbyid, Id);
            if (GetbyId != null)
            {
                response.response.IsSuccess = true;
                response.role = GetbyId;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public RoleResponse Insert(RoleModel param)
        {
            RoleResponse response = new RoleResponse();
            response = new RoleResponse()
            {
                role = new RoleModel(),
                response = new ResponseModel()
            };
            HelperClass helper = new HelperClass();
            var Add = _repos.Insert(insert, new
            {
                @Role = param.Role,
                @Description = param.Description,
                @IsActive= param.IsActive,
            });
            if (Add != false)
            {
                response.response.IsSuccess = true;
                response.role = param;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public RoleResponse Update(RoleModel param)
        {
            RoleResponse response = new RoleResponse();
            response = new RoleResponse()
            {
                role = new RoleModel(),
                response = new ResponseModel()
            };
            HelperClass helper = new HelperClass();
            var Edit = _repos.Update(update, new
            {
                @Id = param.Id,
                @Role = param.Role,
                @Description = param.Description,
                @IsActive= param.IsActive,

            });
            if (Edit != false)
            {
                response.response.IsSuccess = true;
                response.role = param;
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

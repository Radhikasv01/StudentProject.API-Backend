using Dapper;
using Npgsql;
using Student.Interface;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Student.Service
{
    public class StudentService : IStudent
    {
       
        private readonly string _Connection;
        private readonly IRepository<StudentModel> _repos;
        private string _db;

        public StudentService(IRepository<StudentModel> repos)
        {
            _repos = repos;
            _db = Settings.ConnectionString;
        }
        public IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_db);
            }
        }

        const string get_all = @"SELECT * FROM public.""Students""";

        const string getbyid = @"SELECT * FROM public.""Students"" WHERE ""Id""= @Id";

        const string insert = @"INSERT INTO public.""Students""(""StudentName"", ""StudentEmail"",""StudentDepartment"",""StudentJoiningDate"",""StudentWeight"",""StudentHeight"",""BloodGroup"",""IsActive"") VALUES (@StudentName,@StudentEmail,@StudentDepartment,@StudentJoiningDate,@StudentWeight,@StudentHeight, @BloodGroup,@IsActive) RETURNING *";

        const string update = @"UPDATE public.""Students"" SET ""StudentName""=@StudentName, ""StudentEmail""=@StudentEmail,""StudentDepartment""=@StudentDepartment,""StudentJoiningDate""=@StudentJoiningDate,""StudentWeight""=@StudentWeight,""StudentHeight""=@StudentHeight,""BloodGroup""=@BloodGroup,""IsActive""=@IsActive WHERE ""Id""=@Id RETURNING *";

        const string delete = @"update  public.""Students"" SET ""IsActive""=false WHERE ""Id""= @Id";
        const string getAllPaged = @" SELECT * FROM public.""Students"" ORDER BY ""Id"" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
       

        public ListStudentModelResponse GetAll(int pageNumber, int pageSize)
        {
            var Offset = (pageNumber - 1) * pageSize;
            var registrations = _repos.GetByFilter(getAllPaged, new { Offset = Offset, PageSize = pageSize }).ToList();

            ListStudentModelResponse response = new ListStudentModelResponse();
            response = new ListStudentModelResponse()
            {
                students = new List<StudentModel>(),
                response = new ResponseModel()
            };
            if (pageNumber > 0 && pageSize > 0)
            {
                const string getTotalCount = @"SELECT COUNT(*) FROM public.""Students""";
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var x = dbConnection.ExecuteScalar(getTotalCount);
                    var count = Convert.ToInt32(x);
                    if (count > 0)
                    {
                        response.response.IsSuccess = true;
                        response.TotalCount = count;
                        response.students = registrations;
                    }
                }
            }
            else
            {
                List<StudentModel> Getall = _repos.GetAllID(get_all);
                if (Getall != null && Getall.Count > 0)
                {
                    response.response.IsSuccess = true;
                    response.students = Getall;
                }
                else
                {
                    response.response.IsSuccess = false;
                    response.response.Message = "No Records available";
                }
            }
            return response;
        }

        public StudentModelResponse GetById(int Id)
        {
            StudentModelResponse response = new StudentModelResponse();
            response = new StudentModelResponse()
            {
                student = new StudentModel(),
                response = new ResponseModel()
            };
            StudentModel GetbyId = _repos.GetById(getbyid, Id);
            if (GetbyId != null)
            {
                response.response.IsSuccess = true;
                response.student = GetbyId;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public StudentModelResponse Insert(StudentModel param)
        {

            StudentModelResponse response = new StudentModelResponse();
            response = new StudentModelResponse()
            {
                student = new StudentModel(),
                response = new ResponseModel()
            };
            HelperClass helper = new HelperClass();
            var Add = _repos.Insert(insert, new
            {
                @StudentName=param.StudentName,
                @StudentEmail=param.StudentEmail,
                @StudentDepartment=param.StudentDepartment,
                @StudentJoiningDate=param.StudentJoiningDate,
                @StudentWeight=param.StudentWeight,
                @StudentHeight=param.StudentHeight,
                @BloodGroup=param.BloodGroup,
                @IsActive=param.IsActive,

            });
            if (Add != false)
            {
                response.response.IsSuccess = true;
                response.student = param;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public StudentModelResponse Update(StudentModel param)
        {
            StudentModelResponse response = new StudentModelResponse();
            response = new StudentModelResponse()
            {
                student = new StudentModel(),
                response = new ResponseModel()
            };
            HelperClass helper = new HelperClass();
            var Edit = _repos.Update(update, new
            {
                @Id=param.Id,
                @StudentName = param.StudentName,
                @StudentEmail = param.StudentEmail,
                @StudentDepartment = param.StudentDepartment,
                @StudentJoiningDate = param.StudentJoiningDate,
                @StudentWeight = param.StudentWeight,
                @StudentHeight = param.StudentHeight,
                @BloodGroup = param.BloodGroup,
                @IsActive= param.IsActive,

            });
            if (Edit != false)
            {
                response.response.IsSuccess = true;
                response.student = param;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public StudentModelResponse Delete(int Id)
        {
           
            StudentModelResponse response = new StudentModelResponse();
            response = new StudentModelResponse()
            {
                student = new StudentModel(),
                response = new ResponseModel()
            };
            const string getTotalCount = @"SELECT COUNT(*) FROM public.""BooksModels"" WHERE ""StudentId""=@Id";
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var x = dbConnection.ExecuteScalar(getTotalCount, new {@Id=Id});
                var count = Convert.ToInt32(x);
                if (count > 0)
                {
                    response.response.IsSuccess = true;
                    response.response.Message = "Books Are Available in the Student";
                   return response;
                }
            }
            var Remove = _repos.Delete(delete, new { @Id = Id });
            if (Remove)
            {
                response.response.IsSuccess = true;
                response.response.Message = "Deleted Successfully";
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

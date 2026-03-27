using Dapper;
using Npgsql;
using Student.Interface;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Student.Service
{
    public class StudentBookService : IStudentBooks
    {

        private readonly string _Connection;
        private readonly IRepository<StudentBooksModel> _repos;
        private string _db;

        public StudentBookService(IRepository<StudentBooksModel> repos)
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
        const string get_all = @"SELECT * FROM public.""BooksModels""";
        const string getbyid = @"SELECT * FROM public.""BooksModels"" WHERE ""Id""= @Id";

        const string insert = @"INSERT INTO public.""BooksModels""(""StudentId"", ""BookName"",""IsActive"") VALUES (@StudentId, @BookName,@IsActive) RETURNING *";

        const string update = @"UPDATE public.""BooksModels"" SET ""StudentId""=@StudentId, ""BookName""=@BookName,""IsActive""=@IsActive WHERE ""Id""=@Id RETURNING *";

        const string delete = @"update  public.""BooksModels"" SET ""IsActive""=false WHERE ""Id""= @Id";
        const string Getbyfiler = @"SELECT s.""StudentName"",b.* FROM public.""BooksModels"" b left join public.""Students"" s on s.""Id"" = b.""StudentId"" WHERE b.""StudentId"" = @Id";
        const string getAllPaged = @"SELECT * FROM public.""BooksModels"" ORDER BY ""Id"" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        public ListStudentBooksResponse GetAll(int pageNumber, int pageSize)
        {
            var Offset = (pageNumber - 1) * pageSize;
            ListStudentBooksResponse response = new ListStudentBooksResponse();
            response = new ListStudentBooksResponse()
            {
                allbooks = new List<StudentBooksModel>(),
                response = new ResponseModel()
            };
            if (pageNumber > 0 && pageSize > 0)
            {
                List<StudentBooksModel> registrations = _repos.GetByFilter(getAllPaged, new { Offset = Offset, PageSize = pageSize }).ToList();
                const string getTotalCount = @"SELECT COUNT(*) FROM public.""BooksModels""";
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var x = dbConnection.ExecuteScalar(getTotalCount);
                    var count = Convert.ToInt32(x);
                    if (count > 0)
                    {
                        response.response.IsSuccess = true;
                        response.TotalCount = count;
                        response.allbooks = registrations;
                    }
                }
            }
            else
            {
                List<StudentBooksModel> Getall = _repos.GetAllID(get_all);
                if (Getall != null && Getall.Count > 0)
                {
                    response.response.IsSuccess = true;
                    response.allbooks = Getall;
                }
                else
                {
                    response.response.IsSuccess = false;
                    response.response.Message = "No Records available";
                }
            }
            return response;
        }

        public StudentBooksResponse GetById(int Id)
        {
            StudentBooksResponse response = new StudentBooksResponse();
            response = new StudentBooksResponse()
            {
                books = new StudentBooksModel(),
                response = new ResponseModel()
            };
            StudentBooksModel GetbyId = _repos.GetById(getbyid, Id);
            if (GetbyId != null)
            {
                response.response.IsSuccess = true;
                response.books = GetbyId;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public StudentBooksResponse Insert(StudentBooksModel param)
        {
            StudentBooksResponse response = new StudentBooksResponse();
            response = new StudentBooksResponse()
            {
                books = new StudentBooksModel(),
                response = new ResponseModel()
            };
            HelperClass helper = new HelperClass();
            var Add = _repos.Insert(insert, new
            {
                @StudentId = param.StudentId,
                @BookName = param.BookName,
                @IsActive = param.IsActive,

            });
            if (Add != false)
            {
                response.response.IsSuccess = true;
                response.books = param;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;

        }

        public StudentBooksResponse Update(StudentBooksModel param)
        {
            StudentBooksResponse response = new StudentBooksResponse();
            response = new StudentBooksResponse()
            {
                books = new StudentBooksModel(),
                response = new ResponseModel()
            };
            HelperClass helper = new HelperClass();
            var Edit = _repos.Update(update, new
            {
                @Id = param.Id,
                @StudentId = param.StudentId,
                @BookName = param.BookName,
                @IsActive = param.IsActive,


            });
            if (Edit != false)
            {
                response.response.IsSuccess = true;
                response.books = param;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public StudentBooksResponse Delete(int Id)
        {
            StudentBooksResponse response = new StudentBooksResponse();
            response = new StudentBooksResponse()
            {
                books = new StudentBooksModel(),
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

        public ListStudentBooksResponse GetByFilter(int Id, int pageNumber, int pageSize)
        {
            ListStudentBooksResponse response = new ListStudentBooksResponse();
            response = new ListStudentBooksResponse()
            {
                allbooks = new List<StudentBooksModel>(),
                response = new ResponseModel()

            };
            using(IDbConnection connection = null)
            {
                const string Getbyfiler = @"SELECT s.""StudentName"",b.* FROM public.""BooksModels"" b left join public.""Students"" s on s.""Id"" = b.""StudentId"" WHERE b.""StudentId"" = @Id";
                const string count = @"Select Count(*) from public.""BooksModels"" where ""StudentId""=""Id""";

                var Offset = (pageNumber - 1) * pageSize;
                var registrations = _repos.GetByFilter(getAllPaged, new { Offset = Offset, PageSize = pageSize }).ToList();
            }
            List<StudentBooksModel> getbyfilter = _repos.GetByFilter(Getbyfiler, new { @Id = Id }).ToList();
            if (getbyfilter != null && getbyfilter.Count > 0)
            {
                response.response.IsSuccess = true;
                response.allbooks = getbyfilter;
                response.response.Message = "Success";
                response.TotalCount = getbyfilter.Count;
                
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

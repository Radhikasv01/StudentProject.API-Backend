using OfficeOpenXml;
using Student.Interface;
using Student.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Npgsql;
using Dapper;


namespace Student.Service
{
    public class RegisterService : IRegister
    {
        private readonly string _Connection;
        private readonly IRepository<RegisterModel> _repos;
        private string _db;
        public RegisterService(IRepository<RegisterModel> repos)
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
        const string get_all = @"SELECT * FROM public.""RegisterModels""";
        const string getbyid = @"SELECT * FROM public.""RegisterModels"" WHERE ""Id""= @Id";

        const string insert = @"INSERT INTO public.""RegisterModels""(""UserName"", ""EmailAddress"", ""Password"", ""Mobile"", ""Date"", ""Address"", ""Pincode"", ""RoleId"",""IsActive"") VALUES (@UserName, @EmailAddress, @Password, @Mobile, @Date, @Address, @Pincode, @RoleId,@IsActive) RETURNING *";

        const string update = @"UPDATE public.""RegisterModels"" SET ""UserName""=@UserName,""EmailAddress""=@EmailAddress,  ""Password""=@Password, ""Mobile""=@Mobile,""Date""=@Date, ""Address""=@Address, ""Pincode""=@Pincode, ""RoleId""=@RoleId, ""IsActive""=@IsActive WHERE ""Id""=@Id RETURNING *";

        const string delete = @"UPDATE public.""RegisterModels"" SET ""IsActive""=false  WHERE ""Id""= @Id";

        //const string getByUserName = @"SELECT COUNT(*) FROM public.""RegisterModel"" WHERE ""UserName"" = @UserName";
        const string getAllPaged = @" SELECT * FROM public.""RegisterModels"" ORDER BY ""Id"" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        //const string getTotalCount = @"SELECT COUNT(*) FROM public.""RegisterModel""";
        public RegisterResponse Delete(int Id)
        {
            RegisterResponse response = new RegisterResponse();
            response = new RegisterResponse()
            {
                register = new RegisterModel(),
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

        public byte[] ExportToExcel()
        {
            var registrations = _repos.GetAllID(get_all); // Fetch all records
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Registrations");

                // Add headers
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "RoleId";
                worksheet.Cells[1, 3].Value = "UserName";
                worksheet.Cells[1, 4].Value = "EmailAddress";
                worksheet.Cells[1, 5].Value = "Mobile";
                worksheet.Cells[1, 6].Value = "Date";
                worksheet.Cells[1, 7].Value = "Address";
                worksheet.Cells[1, 8].Value = "Pincode";

                // Add data
                for (var i = 0; i < registrations.Count; i++)
                {
                    var reg = registrations[i];
                    worksheet.Cells[i + 2, 1].Value = reg.Id;
                    worksheet.Cells[i + 2, 2].Value = reg.RoleId;
                    worksheet.Cells[i + 2, 3].Value = reg.UserName;
                    worksheet.Cells[i + 2, 4].Value = reg.EmailAddress;
                    worksheet.Cells[i + 2, 5].Value = reg.Mobile;
                    worksheet.Cells[i + 2, 6].Value = reg.Date;
                    worksheet.Cells[i + 2, 7].Value = reg.Address;
                    worksheet.Cells[i + 2, 8].Value = reg.Pincode;
                }

                return package.GetAsByteArray();
            }
        }

        //public ListRegisterResponse GetAll(int pageNumber, int pageSize)
        //{
        //    var Offset = (pageNumber - 1) * pageSize;
        //    var registrations = _repos.GetByFilter(getAllPaged, new { Offset = Offset, PageSize = pageSize }).ToList();

        //    ListRegisterResponse response = new ListRegisterResponse();
        //    response = new ListRegisterResponse()
        //    {
        //        register = new List<RegisterModel>(),
        //        response = new ResponseModel()

        //    };
        //    if (pageNumber > 0 && pageSize > 0)
        //    {
        //        const string getTotalCount = @"SELECT COUNT(*) FROM public.""RegisterModels""";
        //        using (IDbConnection dbConnection = Connection)
        //        {
        //            dbConnection.Open();
        //            var x = dbConnection.ExecuteScalar(getTotalCount);
        //            var count = Convert.ToInt32(x);
        //            if (count > 0)
        //            {
        //                response.response.IsSuccess = true;
        //                response.TotalCount = count;
        //                response.register = registrations;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        List<RegisterModel> Getall = _repos.GetAllID(get_all);
        //        if (registrations != null)
        //        {
        //            response.response.IsSuccess = true;
        //            response.register = Getall;
        //        }
        //        else
        //        {
        //            response.response.IsSuccess = false;
        //            response.response.Message = "No Records available";
        //        }
        //    }
        //    return response;
        //}

        public ListRegisterResponse GetAll(int pageNumber, int pageSize)
        {
            var Offset = (pageNumber - 1) * pageSize;

            var registrations =
                _repos.GetByFilter(
                    getAllPaged,
                    new { Offset = Offset, PageSize = pageSize }
                ).ToList();

            ListRegisterResponse response = new ListRegisterResponse()
            {
                register = new List<RegisterModel>(),
                response = new ResponseModel()
            };

            if (pageNumber > 0 && pageSize > 0)
            {
                const string getTotalCount =
                    @"SELECT COUNT(*) FROM public.""RegisterModels""";

                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();

                    var x = dbConnection.ExecuteScalar(getTotalCount);

                    var count = Convert.ToInt32(x);

                    if (count > 0)
                    {
                        // IMPORTANT FIX
                        HelperClass helper = new HelperClass();

                        foreach (var user in registrations)
                        {
                            if (!string.IsNullOrEmpty(user.Password))
                            {
                                user.Password =
                                    helper.DecryptPassword(user.Password);
                            }
                        }

                        response.response.IsSuccess = true;
                        response.TotalCount = count;
                        response.register = registrations;
                    }
                }
            }

            return response;
        }

        public RegisterResponse GetById(int Id)
        {
            RegisterResponse response = new RegisterResponse();
            response = new RegisterResponse()
            {
                register = new RegisterModel(),
                response = new ResponseModel()
            };
            RegisterModel GetbyId = _repos.GetById(getbyid, Id);
            if (GetbyId != null)
            {
                HelperClass helper = new HelperClass();
                GetbyId.Password = helper.DecryptPassword(GetbyId.Password);
                response.response.IsSuccess = true;
                response.register = GetbyId;
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        public RegisterResponse Insert(RegisterModel param)
        {
            RegisterResponse response = new RegisterResponse();
            response = new RegisterResponse()
            {
                register = new RegisterModel(),
                response = new ResponseModel()
            };
            const string getByUserName = @"SELECT COUNT(*) FROM public.""RegisterModels"" WHERE ""UserName"" = @UserName";
            using IDbConnection dbConnection = Connection;
            Connection.Open();
            var x = dbConnection.ExecuteScalar(getByUserName, new { UserName = param.UserName });
            var count = Convert.ToInt32(x);
            if (count > 0)
            {
                response.response.IsSuccess = false;
                response.response.Message = "Username already exists. Please choose a different username.";
                return response;
            }

            HelperClass helper = new HelperClass();
            var Add = _repos.Insert(insert, new
            {

                @UserName = param.UserName,
                @EmailAddress = param.EmailAddress,
                @Password = helper.EncryptPassword(param.Password),
                @Mobile = param.Mobile,
                @Date = param.Date,
                @Address = param.Address,
                @Pincode = param.Pincode,
                @RoleId = param.RoleId,
                @IsActive = param.IsActive,

            });
            if (Add != false)
            {
                response.response.IsSuccess = true;
                response.register = param;
                response.response.Message = "Registeration Successful";
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "No Records Available";
            }
            return response;
        }

        //        public RegisterResponse Update(RegisterModel param)
        //        {
        //            RegisterResponse response = new RegisterResponse();
        //            response = new RegisterResponse()
        //            {
        //                register = new RegisterModel(),
        //                response = new ResponseModel()
        //            };
        //            //var count = _repos.GetByuserName(getByUserName, new { UserName = param.UserName, Id = param.Id });
        //            //if (count > 0)
        //            //{
        //            //    ;
        //            //    response.response.IsSuccess = false;
        //            //    response.response.Message = "Username already exists. Please choose a different username.";
        //            //    return response;
        //            //}

        //            HelperClass helper = new HelperClass();
        //            var Edit = _repos.Update(update, new
        //            {
        //                @Id = param.Id,
        //                @UserName = param.UserName,
        //                @EmailAddress = param.EmailAddress,
        //                @Password = helper.EncryptPassword(param.Password),
        //                @Mobile = param.Mobile,
        //                @Date = param.Date,
        //                @Address = param.Address,
        //                @Pincode = param.Pincode,
        //                @RoleId = param.RoleId,
        //                @IsActive = param.IsActive,

        //            });
        //            if (Edit != false)
        //            {
        //                response.response.IsSuccess = true;
        //                response.register = param;
        //            }
        //            else
        //            {
        //                response.response.IsSuccess = false;
        //                response.response.Message = "No Records Available";
        //            }
        //            return response;
        //        }
        //    }
        //}


        public RegisterResponse Update(RegisterModel param)
        {
            RegisterResponse response = new RegisterResponse()
            {
                register = new RegisterModel(),
                response = new ResponseModel()
            };

            HelperClass helper = new HelperClass();

            // Step 1 — Get existing record
            RegisterModel existingUser =
                _repos.GetById(getbyid, param.Id);

            if (existingUser == null)
            {
                response.response.IsSuccess = false;
                response.response.Message = "User not found";
                return response;
            }

            // Step 2 — Check password changed or not

            string decryptedOldPassword =
                helper.DecryptPassword(existingUser.Password);

            string finalPassword;

            if (param.Password == decryptedOldPassword)
            {
                // Password not changed
                finalPassword = existingUser.Password;
            }
            else
            {
                // Password changed
                finalPassword =
                    helper.EncryptPassword(param.Password);
            }

            // Step 3 — Update

            var Edit = _repos.Update(update, new
            {
                @Id = param.Id,
                @UserName = param.UserName,
                @EmailAddress = param.EmailAddress,
                @Password = finalPassword,
                @Mobile = param.Mobile,
                @Date = param.Date,
                @Address = param.Address,
                @Pincode = param.Pincode,
                @RoleId = param.RoleId,
                @IsActive = param.IsActive,
            });

            if (Edit)
            {
                response.response.IsSuccess = true;
                response.register = param;
                response.response.Message = "Updated Successfully";
            }
            else
            {
                response.response.IsSuccess = false;
                response.response.Message = "Update Failed";
            }

            return response;
        }
    }
}

//using OfficeOpenXml;
//using Student.Interface;
//using Student.Model;
//using System.Data;
//using Npgsql;
//using Dapper;
//using System.Linq;

//namespace Student.Service
//{
//    public class RegisterService : IRegister
//    {
//        private readonly string _Connection;
//        private readonly IRepository<RegisterModel> _repos;
//        public RegisterService(IRepository<RegisterModel> repos)
//        {
//            _repos = repos;
//            _Connection = Settings.ConnectionString;
//        }

//        public IDbConnection Connection
//        {
//            get
//            {
//                return new NpgsqlConnection(_Connection);
//            }
//        }

//        public RegisterResponse Delete(int Id)
//        {
//            RegisterResponse response = new RegisterResponse();
//            response = new RegisterResponse()
//            {
//                register = new RegisterModel(),
//                response = new ResponseModel()
//            };

//            using (IDbConnection dbConnection = Connection)
//            {
//                dbConnection.Open();
//                var result = dbConnection.Execute("CALL DeleteRegister(@Id)", new { Id });
//                if (result != null)
//                {
//                    response.response.IsSuccess = true;
//                    response.response.Message = "Data has been deleted";
//                }
//                else
//                {
//                    response.response.IsSuccess = false;
//                    response.response.Message = "No Records Available";
//                }
//            }

//            return response;
//        }

//        public byte[] ExportToExcel()
//        {
//            using (IDbConnection dbConnection = Connection)
//            {
//                var registrations = dbConnection.Query<RegisterModel>("SELECT * FROM GetAllRegisters()").ToList();

//                using (var package = new ExcelPackage())
//                {
//                    var worksheet = package.Workbook.Worksheets.Add("Registrations");

//                    // Add headers
//                    worksheet.Cells[1, 1].Value = "Id";
//                    worksheet.Cells[1, 2].Value = "RoleId";
//                    worksheet.Cells[1, 3].Value = "UserName";
//                    worksheet.Cells[1, 4].Value = "EmailAddress";
//                    worksheet.Cells[1, 5].Value = "Mobile";
//                    worksheet.Cells[1, 6].Value = "Date";
//                    worksheet.Cells[1, 7].Value = "Address";
//                    worksheet.Cells[1, 8].Value = "Pincode";

//                    // Add data
//                    for (var i = 0; i < registrations.Count; i++)
//                    {
//                        var reg = registrations[i];
//                        worksheet.Cells[i + 2, 1].Value = reg.Id;
//                        worksheet.Cells[i + 2, 2].Value = reg.RoleId;
//                        worksheet.Cells[i + 2, 3].Value = reg.UserName;
//                        worksheet.Cells[i + 2, 4].Value = reg.EmailAddress;
//                        worksheet.Cells[i + 2, 5].Value = reg.Mobile;
//                        worksheet.Cells[i + 2, 6].Value = reg.Date;
//                        worksheet.Cells[i + 2, 7].Value = reg.Address;
//                        worksheet.Cells[i + 2, 8].Value = reg.Pincode;
//                    }

//                    return package.GetAsByteArray();
//                }
//            }
//        }

//        public ListRegisterResponse GetAll(int pageNumber, int pageSize)
//        {
//            var response = new ListRegisterResponse
//            {
//                register = new List<RegisterModel>(),
//                response = new ResponseModel()
//            };

//            using (var dbConnection = Connection)
//            {
//                dbConnection.Open();
//                var registrations = dbConnection.Query<RegisterModel>("SELECT * FROM getallregistrations()").ToList();

//                if (registrations.Any())
//                {
//                    response.response.IsSuccess = true;
//                    response.register = registrations;
//                    response.TotalCount = registrations.Count; // Since `getallregistrations()` returns all records
//                }
//                else
//                {
//                    response.response.IsSuccess = false;
//                    response.response.Message = "No Records available";
//                }
//            }

//            return response;
//        }
//        public RegisterResponse GetById(int Id)
//        {
//            RegisterResponse response = new RegisterResponse
//            {
//                register = new RegisterModel(),
//                response = new ResponseModel()
//            };

//            using (IDbConnection dbConnection = Connection)
//            {
//                dbConnection.Open();

//                // Call the procedure and map the result to RegisterModel
//                var reg = dbConnection.QuerySingleOrDefault<RegisterModel>(
//                    "CALL getregisterbyid(@Id)", new { Id });

//                if (reg != null)
//                {
//                    response.response.IsSuccess = true;
//                    response.register = reg;
//                }
//                else
//                {
//                    response.response.IsSuccess = false;
//                    response.response.Message = "No Records Available";
//                }
//            }

//            return response;
//        }

//        public RegisterResponse Insert(RegisterModel param)
//        {
//            RegisterResponse response = new RegisterResponse()
//            {
//                register = new RegisterModel(),
//                response = new ResponseModel()
//            };

//            // Check if username exists
//            using (IDbConnection dbConnection = Connection)
//            {
//                var count = dbConnection.ExecuteScalar<int>(
//                    "SELECT COUNT(*) FROM public.\"RegisterModel\" WHERE \"UserName\" = @UserName",
//                    new { param.UserName }
//                );

//                if (count > 0)
//                {
//                    response.response.IsSuccess = false;
//                    response.response.Message = "Username already exists. Please choose a different username.";
//                    return response;
//                }

//                // Insert new record
//                var result = dbConnection.Execute("CALL InsertRegister(@UserName, @EmailAddress, @Password, @Mobile, @Date, @Address, @Pincode, @RoleId, @IsActive)",
//                    new
//                    {
//                        @UserName = param.UserName,
//                        @EmailAddress = param.EmailAddress,
//                        @Password = param.Password,
//                        @Mobile = param.Mobile,
//                        @Date = param.Date,
//                        @Address = param.Address,
//                        @Pincode = param.Pincode,  // Ensure this is an integer
//                        @RoleId = param.RoleId,
//                        @IsActive = param.IsActive
//                    });

//                if (result != null)
//                {
//                    response.response.IsSuccess = true;
//                    response.register = param;
//                    response.response.Message = "Registration Successful";
//                }
//                else
//                {
//                    response.response.IsSuccess = false;
//                    response.response.Message = "Registration failed";
//                }
//            }

//            return response;
//        }

//        public RegisterResponse Update(RegisterModel param)
//        {
//            RegisterResponse response = new RegisterResponse()
//            {
//                register = new RegisterModel(),
//                response = new ResponseModel()
//            };

//            using (IDbConnection dbConnection = Connection)
//            {
//                var result = dbConnection.Execute("CALL UpdateRegister(@Id, @UserName, @EmailAddress, @Password, @Mobile, @Date, @Address, @Pincode, @RoleId, @IsActive)",
//                    new
//                    {
//                        @Id = param.Id,
//                        @UserName = param.UserName,
//                        @EmailAddress = param.EmailAddress,
//                        @Password = param.Password,
//                        @Mobile = param.Mobile,
//                        @Date = param.Date,
//                        @Address = param.Address,
//                        @Pincode = param.Pincode,  // Ensure this is an integer
//                        @RoleId = param.RoleId,
//                        @IsActive = param.IsActive
//                    });

//                if (result!=null)
//                {
//                    response.response.IsSuccess = true;
//                    response.register = param;
//                    response.response.Message = "updated Successful";
//                }
//                else
//                {
//                    response.response.IsSuccess = false;
//                    response.response.Message = "Update failed";
//                }
//            }

//            return response;
//        }
//    }
//}



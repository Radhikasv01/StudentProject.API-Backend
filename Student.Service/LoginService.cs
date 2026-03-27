using Microsoft.Azure.Documents;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Student.Interface;
using Student.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Student.Service
{
    public class LoginService : ILogin
    {
        private readonly IRepository<RegisterModel> _repos;

        private IConfiguration _config;
       

        public LoginService(IRepository<RegisterModel> repos, IConfiguration config)
        {

            _repos = repos;
            _config = config;
        }

        public const string Loginauth = @"Select * FROM public.""RegisterModels"" WHERE ""UserName""=@UserName and ""IsActive""=true";

        public ResponseToken Authreg(LoginModel loginuser)
        {
            ResponseToken responseToken = new ResponseToken()
            {
                response = new ResponseModel(),
                login = new LoginModel()
            };

            var login = Authenticate(loginuser);
            if (login != null && login.UserName != null)
            {
                var token = GenerateToken(login);
                responseToken.login.Token = "Bearer " + token;
                responseToken.login.Id = login.Id;
                responseToken.login.Role_Id = login.RoleId;
                responseToken.login.UserName = login.UserName;
                responseToken.login.Password = login.Password;
                responseToken.response.Message = "Login Successful Authenticate Successful";
                responseToken.response.IsSuccess = true;
                return responseToken;

            }
            else
            {
                responseToken.response.IsSuccess = false;
                responseToken.response.Message = "Enter Valid UserName ID and Password";
            }
            return responseToken;
        }

        private string GenerateToken(Model.RegisterModel user)
        {
            string strData = _config["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strData));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.UserName),
                 new Claim(ClaimTypes.UserData,user.Password),
                 new Claim(ClaimTypes.Role,user.RoleId.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);



            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Model.RegisterModel Authenticate(LoginModel login)
        {
            HelperClass helperClass = new HelperClass();
            var result = _repos.GetByFilter(Loginauth, new { @UserName = login.UserName }).FirstOrDefault();
            if (result != null)
            {
                var decryptpwd = helperClass.DecryptPassword(result.Password);
                if (login.UserName == result.UserName && login.Password == decryptpwd)
                {
                    return result;
                }
            }
            return null;
        }
    }
}

using Serilog;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Interface
{
    public interface ILogin
    {
        ResponseToken Authreg(LoginModel loginuser);
    }
}

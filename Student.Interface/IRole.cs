using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Interface
{
    public interface IRole
    {
        ListRoleResponse GetAll();
        RoleResponse GetById(int Id);
        RoleResponse Insert(RoleModel param);
        RoleResponse Update(RoleModel param);
        RoleResponse Delete(int Id);
    }
}

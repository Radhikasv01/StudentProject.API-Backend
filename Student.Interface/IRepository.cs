using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Interface
{
    public interface IRepository<T>
    {
        List<T> GetAllID(string query);
        T GetById(string getidquery, int Id);
        bool Insert(string Insertquery, object param);
        bool Update(string Updatequery, object param);
        bool Delete(string Deletequery, object param);
        IEnumerable<T> GetByFilter(string query, object param);
        

       


    }
}

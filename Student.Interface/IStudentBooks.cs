using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Interface
{
    public interface IStudentBooks
    {
        ListStudentBooksResponse GetAll(int pageNumber, int pageSize);
        StudentBooksResponse GetById(int Id);
        StudentBooksResponse Insert(StudentBooksModel param);
        StudentBooksResponse Update(StudentBooksModel param);
        StudentBooksResponse Delete(int Id);
        ListStudentBooksResponse GetByFilter(int Id, int pageNumber, int pageSize);
    }
}

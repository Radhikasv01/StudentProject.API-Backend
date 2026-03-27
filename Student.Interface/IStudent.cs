using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Interface
{
    public interface IStudent
    {
        ListStudentModelResponse GetAll(int pageNumber, int pageSize);
        StudentModelResponse GetById(int Id);
        StudentModelResponse Insert(StudentModel param);
        StudentModelResponse Update(StudentModel param);
        StudentModelResponse Delete(int Id);
    }
}

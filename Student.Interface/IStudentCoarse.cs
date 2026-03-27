using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Interface
{
    public interface IStudentCoarse
    {
        StudentCoarseResponse Insert (StudentCourse course);
        StudentCoarseResponse Update (StudentCourse course);
        StudentCoarseResponse Delete (int Id);
        ListStudentCourseResponse GetAll();
        StudentCoarseResponse GetById(int Id);    
    }
}

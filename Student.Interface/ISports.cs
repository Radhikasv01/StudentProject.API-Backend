using Student.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Interface
{
    public interface ISports
    {
        ListSportsResponse GetAll();
        SportsResponse Insert(SportsModel sports);
        SportsResponse Update(SportsModel sports);
        SportsResponse Delete(int Id);
        SportsResponse GetById(int Id);
    }
}

using Student.Model;

namespace Student.Interface
{
    public interface IRegister
    {
        ListRegisterResponse GetAll(int pageNumber, int pageSize);
        RegisterResponse GetById(int Id);
        RegisterResponse Insert(RegisterModel param);
        RegisterResponse Update(RegisterModel param);
        RegisterResponse Delete(int Id);
       // public byte[] ExportToExcel();
    }
}

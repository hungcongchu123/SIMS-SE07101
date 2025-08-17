using WebSIMS.DBContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<bool> AddStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> IsStudentCodeExistsAsync(string studentCode, int excludeStudentId = 0);
        Task<bool> IsEmailExistsAsync(string email, int excludeStudentId = 0);
        Task<bool> IsPhoneExistsAsync(string phone, int excludeStudentId = 0);
    }
}

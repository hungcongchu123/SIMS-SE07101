using WebSIMS.DBContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface ICourseService
    {
        Task<List<Courses>> GetAllCoursesAsync();
        Task<Courses?> GetCourseByIdAsync(int id);
        Task<bool> AddCourseAsync(Courses course);
        Task<bool> UpdateCourseAsync(Courses course);
        Task<bool> DeleteCourseAsync(int id);
    }
}
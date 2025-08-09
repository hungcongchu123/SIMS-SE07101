using WebSIMS.DBContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface IStudentCoursesService
    {
        Task<List<StudentCourses>> GetAllAsync();
        Task<StudentCourses?> GetByIdAsync(int id);
        Task<List<StudentCourses>> GetByStudentIdAsync(int studentId);
        Task<List<StudentCourses>> GetByCourseIdAsync(int courseId);
        Task AddAsync(StudentCourses studentCourse);
        Task UpdateAsync(StudentCourses studentCourse);
        Task DeleteAsync(int id);
        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId);
        Task<bool> RemoveStudentFromCourseAsync(int studentId, int courseId);
    }
}

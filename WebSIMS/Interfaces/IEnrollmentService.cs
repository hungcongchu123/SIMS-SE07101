using WebSIMS.DBContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<StudentCourses>> GetAllEnrollmentsAsync();
        Task<List<StudentCourses>> GetEnrollmentsByStudentAsync(int studentId);
        Task<List<StudentCourses>> GetEnrollmentsByCourseAsync(int courseId);
        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId);
        Task<bool> RemoveEnrollmentAsync(int studentId, int courseId);
        Task<bool> UpdateGradeAsync(int studentId, int courseId, string grade);
        Task<StudentCourses?> GetEnrollmentAsync(int studentId, int courseId);
    }
}

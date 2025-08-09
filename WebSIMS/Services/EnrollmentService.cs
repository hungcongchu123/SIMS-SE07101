using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly SIMSDBContext _context;

        public EnrollmentService(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<List<StudentCourses>> GetAllEnrollmentsAsync()
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<List<StudentCourses>> GetEnrollmentsByStudentAsync(int studentId)
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Course)
                .Where(sc => sc.StudentID == studentId)
                .ToListAsync();
        }

        public async Task<List<StudentCourses>> GetEnrollmentsByCourseAsync(int courseId)
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Student)
                .Where(sc => sc.CourseID == courseId)
                .ToListAsync();
        }

        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            // Kiểm tra student và course có tồn tại không
            var student = await _context.StudentsDb.FindAsync(studentId);
            var course = await _context.CoursesDb.FindAsync(courseId);

            if (student == null || course == null)
                return false;

            // Kiểm tra đã đăng ký chưa
            var existingEnrollment = await _context.StudentCoursesDb
                .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == courseId);

            if (existingEnrollment != null)
                return false;

            // Tạo enrollment mới
            var enrollment = new StudentCourses
            {
                StudentID = studentId,
                CourseID = courseId,
                EnrollmentDate = DateTime.Now
            };

            _context.StudentCoursesDb.Add(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = await _context.StudentCoursesDb
                .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == courseId);

            if (enrollment == null)
                return false;

            _context.StudentCoursesDb.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateGradeAsync(int studentId, int courseId, string grade)
        {
            var enrollment = await _context.StudentCoursesDb
                .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == courseId);

            if (enrollment == null)
                return false;

            enrollment.Grade = grade;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StudentCourses?> GetEnrollmentAsync(int studentId, int courseId)
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == courseId);
        }
    }
}

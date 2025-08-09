using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Services
{
    public class StudentCoursesService : IStudentCoursesService
    {
        private readonly SIMSDBContext _context;

        public StudentCoursesService(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<List<StudentCourses>> GetAllAsync()
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<StudentCourses?> GetByIdAsync(int id)
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.StudentCourseID == id);
        }

        public async Task<List<StudentCourses>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Course)
                .Where(sc => sc.StudentID == studentId)
                .ToListAsync();
        }

        public async Task<List<StudentCourses>> GetByCourseIdAsync(int courseId)
        {
            return await _context.StudentCoursesDb
                .Include(sc => sc.Student)
                .Where(sc => sc.CourseID == courseId)
                .ToListAsync();
        }

        public async Task AddAsync(StudentCourses studentCourse)
        {
            studentCourse.EnrollmentDate ??= DateTime.Now;
            _context.StudentCoursesDb.Add(studentCourse);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentCourses studentCourse)
        {
            var existing = await _context.StudentCoursesDb.FindAsync(studentCourse.StudentCourseID);
            if (existing != null)
            {
                existing.StudentID = studentCourse.StudentID;
                existing.CourseID = studentCourse.CourseID;
                existing.EnrollmentDate = studentCourse.EnrollmentDate;
                existing.Grade = studentCourse.Grade;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var studentCourse = await _context.StudentCoursesDb.FindAsync(id);
            if (studentCourse != null)
            {
                _context.StudentCoursesDb.Remove(studentCourse);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            // Kiểm tra xem student và course có tồn tại không
            var student = await _context.StudentsDb.FindAsync(studentId);
            var course = await _context.CoursesDb.FindAsync(courseId);

            if (student == null || course == null)
                return false;

            // Kiểm tra xem đã đăng ký chưa
            var existingEnrollment = await _context.StudentCoursesDb
                .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == courseId);

            if (existingEnrollment != null)
                return false; // Đã đăng ký rồi

            var studentCourse = new StudentCourses
            {
                StudentID = studentId,
                CourseID = courseId,
                EnrollmentDate = DateTime.Now
            };

            await AddAsync(studentCourse);
            return true;
        }

        public async Task<bool> RemoveStudentFromCourseAsync(int studentId, int courseId)
        {
            var enrollment = await _context.StudentCoursesDb
                .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == courseId);

            if (enrollment == null)
                return false;

            _context.StudentCoursesDb.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

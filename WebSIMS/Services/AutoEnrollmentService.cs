using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext;
using WebSIMS.Interfaces;

namespace WebSIMS.Services
{
    public class AutoEnrollmentService
    {
        private readonly SIMSDBContext _context;
        private readonly IEnrollmentService _enrollmentService;

        public AutoEnrollmentService(SIMSDBContext context, IEnrollmentService enrollmentService)
        {
            _context = context;
            _enrollmentService = enrollmentService;
        }

        // Tự động đăng ký student mới vào tất cả courses hiện có
        public async Task AutoEnrollNewStudentAsync(int studentId)
        {
            var courses = await _context.CoursesDb.ToListAsync();
            foreach (var course in courses)
            {
                // Kiểm tra xem đã đăng ký chưa trước khi thêm
                var existingEnrollment = await _context.StudentCoursesDb
                    .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == course.CourseID);
                
                if (existingEnrollment == null)
                {
                    await _enrollmentService.EnrollStudentInCourseAsync(studentId, course.CourseID);
                }
            }
        }

        // Tự động đăng ký tất cả students hiện có vào course mới
        public async Task AutoEnrollNewCourseAsync(int courseId)
        {
            var students = await _context.StudentsDb.ToListAsync();
            foreach (var student in students)
            {
                // Kiểm tra xem đã đăng ký chưa trước khi thêm
                var existingEnrollment = await _context.StudentCoursesDb
                    .FirstOrDefaultAsync(sc => sc.StudentID == student.StudentID && sc.CourseID == courseId);
                
                if (existingEnrollment == null)
                {
                    await _enrollmentService.EnrollStudentInCourseAsync(student.StudentID, courseId);
                }
            }
        }
    }
}

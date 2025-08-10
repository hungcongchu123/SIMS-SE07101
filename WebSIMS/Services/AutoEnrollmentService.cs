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

        // Automatically enroll new students into all existing courses
        public async Task AutoEnrollNewStudentAsync(int studentId)
        {
            var courses = await _context.CoursesDb.ToListAsync();
            foreach (var course in courses)
            {
                // Check if registered before adding
                var existingEnrollment = await _context.StudentCoursesDb
                    .FirstOrDefaultAsync(sc => sc.StudentID == studentId && sc.CourseID == course.CourseID);
                
                if (existingEnrollment == null)
                {
                    await _enrollmentService.EnrollStudentInCourseAsync(studentId, course.CourseID);
                }
            }
        }

        // Automatically register all existing students to the new course
        public async Task AutoEnrollNewCourseAsync(int courseId)
        {
            var students = await _context.StudentsDb.ToListAsync();
            foreach (var student in students)
            {
                // Check if registered before adding
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

using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext.Entities;
using WebSIMS.DBContext;
using WebSIMS.Interfaces;

namespace WebSIMS.Services
{
    public class CourseService : ICourseService
    {
        private readonly SIMSDBContext _context;
        private readonly AutoEnrollmentService _autoEnrollmentService;

        public CourseService(SIMSDBContext context, AutoEnrollmentService autoEnrollmentService)
        {
            _context = context;
            _autoEnrollmentService = autoEnrollmentService;
        }

        public async Task<List<Courses>> GetAllCoursesAsync()
        {
            return await _context.CoursesDb.ToListAsync();
        }

        public async Task<Courses?> GetCourseByIdAsync(int id)
        {
            return await _context.CoursesDb.FindAsync(id);
        }

        public async Task<bool> AddCourseAsync(Courses course)
        {
            try
            {
                course.CreatedAt = DateTime.Now;
                _context.CoursesDb.Add(course);
                await _context.SaveChangesAsync();

                // Automatically register all existing students to the new course
                await _autoEnrollmentService.AutoEnrollNewCourseAsync(course.CourseID);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCourseAsync(Courses course)
        {
            try
            {
                var existingCourse = await _context.CoursesDb.FindAsync(course.CourseID);
                if (existingCourse == null)
                    return false;

                existingCourse.CourseCode = course.CourseCode;
                existingCourse.CourseName = course.CourseName;
                existingCourse.Description = course.Description;
                existingCourse.Credits = course.Credits;
                existingCourse.Department = course.Department;
                existingCourse.CreatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _context.CoursesDb.FindAsync(id);
                if (course == null)
                    return false;

                _context.CoursesDb.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

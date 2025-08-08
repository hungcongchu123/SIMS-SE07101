// Services/Implementations/CourseService.cs
using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using WebSIMS.DBContext;
using WebSIMS.Services.Interfaces;

namespace WebSIMS.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly SIMSDBContext _context;

        public CourseService(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Courses>> GetAllAsync()
        {
            return await _context.CoursesDb.ToListAsync();
        }

        public async Task<Courses?> GetByIdAsync(int id)
        {
            return await _context.CoursesDb.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.CoursesDb.FindAsync(id);
            if (course == null) return false;

            _context.CoursesDb.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddAsync(Courses course)
        {
            course.CreatedAt = DateTime.Now;
            _context.CoursesDb.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Courses course)
        {
            var existing = await _context.CoursesDb.FindAsync(course.CourseID);
            if (existing == null) return;

            existing.CourseCode = course.CourseCode;
            existing.CourseName = course.CourseName;
            existing.Description = course.Description;
            existing.Credits = course.Credits;
            existing.Department = course.Department;
            existing.CreatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}

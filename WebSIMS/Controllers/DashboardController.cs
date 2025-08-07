using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSIMS.BDContext;
using WebSIMS.Models.ViewModels;
using System.Linq;

namespace WebSIMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly SIMSDBContext _context;

        public DashboardController(SIMSDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Student, Faculty")]
        public IActionResult Index()
        {
            var totalStudents = _context.UsersDb.Count(u => u.Role == "Student");
            var totalCourses = _context.CoursesDb.Count();

            var courses = _context.CoursesDb.Select(c => new DashboardViewModel.CourseInfo
            {
                CourseCode = c.CourseCode,
                CourseName = c.CourseName
            }).ToList();

            var model = new DashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalCourses = totalCourses,
                Courses = courses
            };

            return View(model);
        }
    }
}

// Controllers/CourseController.cs
using Microsoft.AspNetCore.Mvc;
using WebSIMS.Services.Interfaces;
using WebSIMS.BDContext.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebSIMS.Controllers
{
    [Authorize(Roles = "Admin,Student,Faculty")] // Toàn bộ controller đều cần đăng nhập và thuộc 1 trong 3 role
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")] // Chỉ Admin được tạo
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")] // Chỉ Admin được tạo
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Courses course)
        {
            if (!ModelState.IsValid) return View(course);

            await _courseService.AddAsync(course);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")] // Chỉ Admin được sửa
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Chỉ Admin được sửa
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Courses course)
        {
            if (!ModelState.IsValid) return View(course);

            await _courseService.UpdateAsync(course);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")] // Chỉ Admin được xóa
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Chỉ Admin được xóa
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _courseService.DeleteAsync(id);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext.Entities;
using WebSIMS.Services.Interfaces;

namespace WebSIMS.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,CourseName,Description,Credits,Department")] Courses course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.AddAsync(course);
                    TempData["SuccessMessage"] = "Course added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("duplicate key") == true)
                    {
                        ModelState.AddModelError("CourseCode", "Course code already exists.");
                        TempData["ErrorMessage"] = "Course code already exists.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"An unexpected error occurred: {ex.Message}";
                    }
                    return View(course);
                }
            }
            TempData["ErrorMessage"] = "Please correct the errors in the form.";
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }

            var course = await _courseService.GetByIdAsync(id.Value);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseCode,CourseName,Description,Credits,Department,CreatedAt")] Courses course)
        {
            if (id != course.CourseID)
            {
                TempData["ErrorMessage"] = "Data mismatch.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.UpdateAsync(course);
                    TempData["SuccessMessage"] = "Course updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["ErrorMessage"] = "The course has been modified by another user. Please try again.";
                    return View(course);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An unexpected error occurred: {ex.Message}";
                    return View(course);
                }
            }
            TempData["ErrorMessage"] = "Please correct the errors in the form.";
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }

            var course = await _courseService.GetByIdAsync(id.Value);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the course: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
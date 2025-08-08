using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Services.Interfaces;

namespace WebSIMS.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public StudentController(IStudentService studentService, ICourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllAsync();
            return View(students);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create()
        {
            var courses = await _courseService.GetAllAsync();
            // ✅ Tạo SelectList sử dụng CourseName làm cả value và text
            ViewBag.Courses = new SelectList(courses, "CourseName", "CourseName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdClaim = User.FindFirst("UserID")?.Value;

                    if (!int.TryParse(userIdClaim, out int userId))
                    {
                        ModelState.AddModelError(string.Empty, "Error: Unable to identify the user. Please log in again.");
                        var courses = await _courseService.GetAllAsync();
                        ViewBag.Courses = new SelectList(courses, "CourseName", "CourseName");
                        return View(student);
                    }

                    student.UserID = userId;
                    await _studentService.AddAsync(student);
                    TempData["SuccessMessage"] = "Successfully added a new student.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("duplicate key") == true)
                    {
                        ModelState.AddModelError(nameof(student.StudentCode), "Student code already exists in the system.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"System error: Could not add student. {ex.Message}");
                    }
                    var courses = await _courseService.GetAllAsync();
                    ViewBag.Courses = new SelectList(courses, "CourseName", "CourseName");
                    return View(student);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error creating student: {ex.Message}");
                    var courses = await _courseService.GetAllAsync();
                    ViewBag.Courses = new SelectList(courses, "CourseName", "CourseName");
                    return View(student);
                }
            }
            var courses_fail = await _courseService.GetAllAsync();
            ViewBag.Courses = new SelectList(courses_fail, "CourseName", "CourseName");
            return View(student);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            var courses = await _courseService.GetAllAsync();
            // ✅ Tạo SelectList sử dụng CourseName làm cả value và text
            ViewBag.Courses = new SelectList(courses, "CourseName", "CourseName");

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingStudent = await _studentService.GetByIdAsync(student.StudentID);
                    if (existingStudent == null)
                    {
                        TempData["ErrorMessage"] = "Student to update not found.";
                        return RedirectToAction("Index");
                    }

                    await _studentService.UpdateAsync(student);
                    TempData["SuccessMessage"] = "Student information updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty, "Student data has been modified. Please try again.");
                    var courses = await _courseService.GetAllAsync();
                    ViewBag.Courses = new SelectList(courses, "CourseName", "CourseName");
                    return View(student);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error updating student: {ex.Message}");
                    var courses = await _courseService.GetAllAsync();
                    ViewBag.Courses = new SelectList(courses, "CourseName", "CourseName");
                    return View(student);
                }
            }
            var courses_fail = await _courseService.GetAllAsync();
            ViewBag.Courses = new SelectList(courses_fail, "CourseName", "CourseName");
            return View(student);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var student = await _studentService.GetByIdAsync(id);
                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student to delete not found.";
                    return RedirectToAction("Index");
                }

                await _studentService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Student deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting student: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
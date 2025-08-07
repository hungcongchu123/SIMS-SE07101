using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Controllers
{
    [Authorize(Roles = "Admin,Faculty")] // Bắt buộc là Admin hoặc Faculty mới truy cập controller này
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllAsync();
            return View(students);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")] // Chỉ Faculty được phép tạo sinh viên
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")] // Chỉ Faculty được phép tạo sinh viên
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy UserID từ claim đã gán khi login
                    var userIdClaim = User.FindFirst("UserID")?.Value;

                    // Kiểm tra hợp lệ và parse UserID
                    if (!int.TryParse(userIdClaim, out int userId))
                    {
                        ModelState.AddModelError(string.Empty, "Không thể xác định người dùng.");
                        return View(student);
                    }

                    // Gán UserID cho sinh viên
                    student.UserID = userId;

                    await _studentService.AddAsync(student);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Lỗi khi tạo sinh viên: {ex.Message}");
                }
            }
            return View(student);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")] //  Chỉ Admin được sửa sinh viên
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] //  Chỉ Admin được sửa sinh viên
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentService.UpdateAsync(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")] //  Chỉ Admin được xóa sinh viên
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] //  Chỉ Admin được xóa sinh viên
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}

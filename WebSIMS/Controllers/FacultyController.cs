using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext.Entities;
using WebSIMS.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System;
using WebSIMS.DBContext;

namespace WebSIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;
        private readonly SIMSDBContext _context;

        public FacultyController(IFacultyService facultyService, SIMSDBContext context)
        {
            _facultyService = facultyService;
            _context = context; // Thêm DbContext vào để xử lý User
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,HireDate")] Faculty faculty, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra email đã tồn tại chưa
                    var existingUser = await _context.UsersDb.FirstOrDefaultAsync(u => u.Username == faculty.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists as a username.");
                        return View(faculty);
                    }

                    // Tạo một đối tượng Users mới
                    var user = new Users
                    {
                        Role = "Faculty",
                        Username = faculty.Email,
                        PasswordHash = password // Trong thực tế nên hash password
                    };

                    // Thêm User vào database trước để có UserID
                    _context.UsersDb.Add(user);
                    await _context.SaveChangesAsync();

                    // Gán UserID đã tạo cho Faculty
                    faculty.UserID = user.UserID;

                    // Thêm Faculty vào database
                    await _facultyService.AddAsync(faculty);

                    TempData["SuccessMessage"] = "Faculty created successfully! Username: " + faculty.Email + ", Password: " + password;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error creating faculty: {ex.Message}";
                    return View(faculty);
                }
            }
            return View(faculty);
        }

        // ... Các phương thức khác (Index, Edit, Delete) vẫn giữ nguyên logic đã sửa
        public async Task<IActionResult> Index()
        {
            var faculties = await _facultyService.GetAllAsync();
            return View(faculties);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var faculty = await _facultyService.GetByIdAsync(id.Value);
            if (faculty == null) return NotFound();
            return View(faculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacultyID,FirstName,LastName,Email,HireDate")] Faculty faculty)
        {
            if (id != faculty.FacultyID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var facultyToUpdate = await _facultyService.GetByIdAsync(id);
                    if (facultyToUpdate == null) return NotFound();

                    facultyToUpdate.FirstName = faculty.FirstName;
                    facultyToUpdate.LastName = faculty.LastName;
                    facultyToUpdate.HireDate = faculty.HireDate;

                    if (facultyToUpdate.Email != faculty.Email)
                    {
                        facultyToUpdate.Email = faculty.Email;
                        if (facultyToUpdate.User != null)
                        {
                            facultyToUpdate.User.Username = faculty.Email;
                        }
                    }

                    await _facultyService.UpdateAsync(facultyToUpdate);
                    TempData["SuccessMessage"] = "Cập nhật giảng viên thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi khi cập nhật: {ex.Message}";
                    return View(faculty);
                }
            }
            return View(faculty);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var faculty = await _facultyService.GetByIdAsync(id.Value);
            if (faculty == null) return NotFound();
            return View(faculty);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _facultyService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa giảng viên thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
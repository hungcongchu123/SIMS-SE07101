using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Controllers
{
    [Authorize(Roles = "Admin,Faculty,Student")]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public EnrollmentController(
            IEnrollmentService enrollmentService,
            IStudentService studentService,
            ICourseService courseService)
        {
            _enrollmentService = enrollmentService;
            _studentService = studentService;
            _courseService = courseService;
        }

        // Hiển thị tất cả đăng ký
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            
            ViewBag.Students = students;
            ViewBag.Courses = courses;
            return View(enrollments);
        }

        // Hiển thị khóa học của 1 sinh viên
        public async Task<IActionResult> StudentEnrollments(int studentId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByStudentAsync(studentId);
            var student = await _studentService.GetStudentByIdAsync(studentId);
            
            ViewBag.Student = student;
            return View(enrollments);
        }

        // Hiển thị sinh viên của 1 khóa học
        public async Task<IActionResult> CourseEnrollments(int courseId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByCourseAsync(courseId);
            var course = await _courseService.GetCourseByIdAsync(courseId);
            
            ViewBag.Course = course;
            return View(enrollments);
        }

        // Đăng ký sinh viên vào khóa học
        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int studentId, int courseId)
        {
            if (courseId == 0)
            {
                // Đăng ký sinh viên vào tất cả khóa học
                var courses = await _courseService.GetAllCoursesAsync();
                var successCount = 0;
                
                foreach (var course in courses)
                {
                    var result = await _enrollmentService.EnrollStudentInCourseAsync(studentId, course.CourseID);
                    if (result) successCount++;
                }
                
                if (successCount > 0)
                    TempData["SuccessMessage"] = $"Successfully enrolled student in {successCount} courses!";
                else
                    TempData["ErrorMessage"] = "Could not enroll student in any courses!";
            }
            else
            {
                // Đăng ký sinh viên vào khóa học cụ thể
                var result = await _enrollmentService.EnrollStudentInCourseAsync(studentId, courseId);
                
                if (result)
                    TempData["SuccessMessage"] = "Enrollment successful!";
                else
                    TempData["ErrorMessage"] = "Enrollment failed!";
            }
            
            return RedirectToAction("Index");
        }

        // Hủy đăng ký
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEnrollment(int studentId, int courseId)
        {
            var result = await _enrollmentService.RemoveEnrollmentAsync(studentId, courseId);
            
            if (result)
                TempData["SuccessMessage"] = "Enrollment cancellation successful!";
            else
                TempData["ErrorMessage"] = "Enrollment cancellation failed!";
            
            return RedirectToAction("Index");
        }

        // Hiển thị form cập nhật điểm
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> UpdateGrade(int studentId, int courseId)
        {
            var enrollment = await _enrollmentService.GetEnrollmentAsync(studentId, courseId);
            if (enrollment == null)
            {
                TempData["ErrorMessage"] = "Enrollment not found!";
                return RedirectToAction("Index");
            }
            
            return View(enrollment);
        }

        // Cập nhật điểm
        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGrade(int studentId, int courseId, string grade)
        {
            var result = await _enrollmentService.UpdateGradeAsync(studentId, courseId, grade);
            
            if (result)
                TempData["SuccessMessage"] = "Grade updated successfully!";
            else
                TempData["ErrorMessage"] = "Grade update failed!";
            
            return RedirectToAction("Index");
        }
    }
}

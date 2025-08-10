using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using WebSIMS.Controllers;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;
using Xunit;

namespace WebSIMS.Tests.Controllers
{
    public class CourseControllerTests
    {
        private readonly Mock<ICourseService> _mockCourseService;
        private readonly CourseController _controller;

        public CourseControllerTests()
        {
            _mockCourseService = new Mock<ICourseService>();
            _controller = new CourseController(_mockCourseService.Object);
            
            // Setup TempData to prevent NullReferenceException
            _controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(), 
                Mock.Of<ITempDataProvider>()
            );
        }

        // ===== CREATE FUNCTION TESTS =====
        
        [Fact]
        public void Create_Get_ReturnsView()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_WithValidCourse_RedirectsToIndex()
        {
            // Arrange
            var course = new Courses 
            { 
                CourseCode = "CS101", 
                CourseName = "Computer Science", 
                Department = "Computer Science",
                Credits = 3
            };
            _mockCourseService.Setup(x => x.AddCourseAsync(course)).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(course);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Create_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            var course = new Courses { CourseCode = "", CourseName = "" };
            _controller.ModelState.AddModelError("CourseCode", "Course Code is required");

            // Act
            var result = await _controller.Create(course);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(course, viewResult.Model);
            Assert.False(_controller.ModelState.IsValid);
        }

        // ===== READ FUNCTION TESTS =====

        [Fact]
        public async Task Index_ReturnsViewWithCourses()
        {
            // Arrange
            var courses = new List<Courses>
            {
                new Courses { CourseID = 1, CourseCode = "CS101", CourseName = "Computer Science", Department = "CS" },
                new Courses { CourseID = 2, CourseCode = "MATH101", CourseName = "Mathematics", Department = "Math" }
            };
            _mockCourseService.Setup(x => x.GetAllCoursesAsync()).ReturnsAsync(courses);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Courses>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        // ===== UPDATE FUNCTION TESTS =====

        [Fact]
        public async Task Edit_Post_WithValidCourse_RedirectsToIndex()
        {
            // Arrange
            var course = new Courses 
            { 
                CourseID = 1, 
                CourseCode = "CS101", 
                CourseName = "Computer Science Updated", 
                Department = "Computer Science" 
            };
            _mockCourseService.Setup(x => x.UpdateCourseAsync(course)).ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(1, course);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        // ===== DELETE FUNCTION TESTS =====

        [Fact]
        public async Task DeleteConfirmed_WithValidId_RedirectsToIndex()
        {
            // Arrange
            _mockCourseService.Setup(x => x.DeleteCourseAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}

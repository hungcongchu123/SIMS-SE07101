namespace WebSIMS.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }

        public List<CourseInfo> Courses { get; set; }
        public string CurrentUserRole { get; set; }// thêm dòng này để hiện thị vai trò của người dùng hiện tại

        public class CourseInfo
        {
            public string CourseCode { get; set; }
            public string CourseName { get; set; }
        }
    }
}

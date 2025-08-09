using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSIMS.DBContext.Entities
{
    public class StudentCourses
    {
        [Key]
        public int StudentCourseID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int CourseID { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public string? Grade { get; set; }

        // Navigation properties
        public Student? Student { get; set; }
        public WebSIMS.DBContext.Entities.Courses? Course { get; set; }
    }
}

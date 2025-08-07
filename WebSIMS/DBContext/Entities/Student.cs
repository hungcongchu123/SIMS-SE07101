using System.ComponentModel.DataAnnotations;
using WebSIMS.DBContext.Entities;

namespace WebSIMS.DBContext.Entities
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        [Required]
        [StringLength(20)]
        public string StudentCode { get; set; } = string.Empty; // ✅ THÊM DÒNG 
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required]
        public string Program { get; set; } = string.Empty;

        public DateTime? EnrollmentDate { get; set; }

        // Foreign key
        public int UserID { get; set; }

        // Navigation property  
        public Users? User { get; set; }
    }
}

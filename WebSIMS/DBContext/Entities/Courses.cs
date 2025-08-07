using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebSIMS.BDContext.Entities
{
    public class Courses
    {
        [Key]
        public int CourseID { get; set; }

        [Column("CourseCode", TypeName = "nvarchar(20)"), Required]
        public string CourseCode { get; set; }

        [Column("CourseName", TypeName = "nvarchar(100)"), Required]
        public string CourseName { get; set; }

        [Column("Description", TypeName = "nvarchar(200)"), AllowNull]
        public string Description { get; set; }

        [Column("Credits", TypeName = "integer")]
        public int Credits { get; set; }

        [Column("Department", TypeName = "nvarchar(100)"), AllowNull]
        public string Department { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }
    }
}

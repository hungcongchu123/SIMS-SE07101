using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSIMS.DBContext.Entities
{
    [Table("Faculties")]
    public class Faculty
    {
        [Key]
        public int FacultyID { get; set; }

        // Loại bỏ Required cho UserID
        // Giả sử UserID có thể null nếu bạn muốn tạo Faculty trước
        // và liên kết User sau
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ không được để trống.")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày tuyển dụng không được để trống.")]
        [Display(Name = "Ngày Tuyển Dụng")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        // Đặt kiểu là Users? để có thể chấp nhận giá trị null
        public Users? User { get; set; }
    }
}
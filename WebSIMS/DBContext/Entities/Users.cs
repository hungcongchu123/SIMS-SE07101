using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebSIMS.DBContext.Entities
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        [Column("Username", TypeName = "nvarchar(30)"), Required]
        public string Username { get; set; }

        [Column("PasswordHash", TypeName = "nvarchar(255)"), Required]
        public string PasswordHash { get; set; }

        [Column("Role", TypeName = "nvarchar(100)"), Required]
        public string Role { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }
    }
}

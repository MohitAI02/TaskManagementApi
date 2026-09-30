using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApi.Models
{
    [Table("roles")] 
    public class Role
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; } 

        [Required]
        [Column("role_name")]
        public required string RoleName { get; set; }
    }
}

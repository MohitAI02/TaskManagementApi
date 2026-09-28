using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        public required string login_id { get; set; }

        [Required]
        public required string password { get; set; }

        [Required]
        public int role_id { get; set; }

        [Required]
        public required string name { get; set; }

        [Required]
        public required string designation { get; set; }

        public Role? Role { get; set; }
    }
}
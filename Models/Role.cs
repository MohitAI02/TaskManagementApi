using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class Role
    {
        [Key]
        public int role_id { get; set; }

        [Required]
        public required string role_name { get; set; }
    }
}
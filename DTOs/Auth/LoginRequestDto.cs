using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public required string loginId { get; set; }

        [Required]
        public required string password { get; set; }
    }
}
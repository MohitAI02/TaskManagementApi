namespace TaskManagementApi.DTOs.Auth
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public required string LoginId { get; set; }
        public required string Name { get; set; }
        public required string Designation { get; set; }
        public required string Role { get; set; }
    }
}
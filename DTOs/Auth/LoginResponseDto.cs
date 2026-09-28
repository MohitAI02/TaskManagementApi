namespace TaskManagementApi.DTOs.Auth
{
    public class LoginResponseDto
    {
        public int userId { get; set; }
        public required string loginId { get; set; }
        public required string name { get; set; }
        public required string designation { get; set; }
        public required string role { get; set; }
    }
}
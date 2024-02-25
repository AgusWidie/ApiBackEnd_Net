namespace APIClinic.Models.DTOs.Request
{
    public class LoginRequest
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? ApiKey { get; set; }
    }
}

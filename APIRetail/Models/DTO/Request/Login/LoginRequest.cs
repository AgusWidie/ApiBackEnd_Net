namespace APIRetail.Models.DTO.Request
{
    public class LoginRequest
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? ApiKey { get; set; }
    }
}

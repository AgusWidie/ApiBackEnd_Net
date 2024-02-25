namespace APIRetail.Models.DTO.Request
{
    public class ChangePasswordRequest
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? PasswordNew { get; set; }

        public string? ApiKey { get; set; }
    }
}

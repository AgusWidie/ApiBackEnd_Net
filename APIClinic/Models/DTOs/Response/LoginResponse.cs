namespace APIClinic.Models.DTOs.Response
{
    public class LoginResponse
    {
        public long? ClinicId { get; set; }

        public long? BranchId { get; set; }

        public string? UserName { get; set; }

        public string? Token { get; set; }

        public string? TokenExpired { get; set; }

        public string? Email { get; set; }

        public string? Message { get; set; }
    }
}

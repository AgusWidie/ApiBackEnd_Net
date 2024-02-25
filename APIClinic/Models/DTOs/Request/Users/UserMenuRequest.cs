namespace APIClinic.Models.DTOs.Request
{
    public class UserMenuRequest
    {

        public long? ClinicId { get; set; }

        public long? BranchId { get; set; }

        public string? UserName { get; set; }
    }
}

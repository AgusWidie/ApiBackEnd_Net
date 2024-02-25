namespace APIClinic.Models.DTOs.Request
{
    public class UserRequest
    {
        public long? Id { get; set; }

        public long? ClinicId { get; set; }

        public long? BranchId { get; set; }

        public string UserName { get; set; } = "";

        public string Name { get; set; } = "";

        public string Password { get; set; } = "";

        public DateTime? PasswordExpired { get; set; }

        public string Description { get; set; } = "";

        public ulong? Active { get; set; }

        public DateTime? LastLogin { get; set; }

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

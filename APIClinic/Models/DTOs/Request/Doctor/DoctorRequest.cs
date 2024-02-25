namespace APIClinic.Models.DTOs.Request
{
    public class DoctorRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public string DoctorName { get; set; } = "";

        public DateOnly? DateOfBirth { get; set; }

        public string? Address { get; set; } = "";

        public string? NoTelephone { get; set; } = "";

        public string MobilePhone { get; set; } = "";

        public string Gender { get; set; } = "";

        public string? Education { get; set; } = "";

        public string? StatusEmployee { get; set; } = "";

        public string StatusDoctor { get; set; } = "";

        public ulong? Active { get; set; }

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

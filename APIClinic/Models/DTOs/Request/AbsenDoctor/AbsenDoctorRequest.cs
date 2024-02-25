namespace APIClinic.Models.DTOs.Request
{
    public class AbsenDoctorRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public long DoctorId { get; set; }

        public string? AbsenType { get; set; } = "";

        public string? Day { get; set; } = "";

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Description { get; set; } = "";

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

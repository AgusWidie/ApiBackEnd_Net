namespace APIClinic.Models.DTOs.Request
{
    public class SpecialistDoctorRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public long DoctorId { get; set; }

        public long SpecialistId { get; set; }

        public string? Description { get; set; } = "";

        public ulong? Active { get; set; }

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

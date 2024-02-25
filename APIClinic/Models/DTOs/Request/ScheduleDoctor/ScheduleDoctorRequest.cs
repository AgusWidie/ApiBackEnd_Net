namespace APIClinic.Models.DTOs.Request
{
    public class ScheduleDoctorRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public long DoctorId { get; set; }

        public long? SpecialistDoctorId { get; set; }

        public string? Day { get; set; } = "";

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public ulong? Active { get; set; }

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }
    }
}

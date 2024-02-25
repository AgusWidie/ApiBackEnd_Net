namespace APIClinic.Models.DTOs.Response
{
    public class SpecialistDoctorResponse
    {
        public long Id { get; set; }

        public long? ClinicId { get; set; }

        public string ClinicName { get; set; } = "";

        public long? BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public long? DoctorId { get; set; }

        public string DoctorName { get; set; } = "";

        public long? SpecialistId { get; set; }

        public string SpecialistName { get; set; } = "";

        public string Description { get; set; } = "";

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

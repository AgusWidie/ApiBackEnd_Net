namespace APIClinic.Models.DTOs.Request
{
    public class ExaminationDoctorRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public long? DoctorId { get; set; }

        public DateTime? ExaminationDate { get; set; }

        public string QueueNo { get; set; } = "";

        public string Inspection { get; set; } = "";

        public string Recipe { get; set; } = "";

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

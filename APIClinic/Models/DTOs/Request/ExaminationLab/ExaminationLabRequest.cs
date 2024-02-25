namespace APIClinic.Models.DTOs.Request
{
    public class ExaminationLabRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public DateTime? ExaminationDate { get; set; }

        public string QueueNo { get; set; } = "";

        public string Description { get; set; } = "";

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

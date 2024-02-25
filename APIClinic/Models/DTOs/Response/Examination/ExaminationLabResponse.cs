namespace APIClinic.Models.DTOs.Response
{
    public class ExaminationLabResponse
    {
        public long Id { get; set; }

        public long? ClinicId { get; set; }

        public string ClinicName { get; set; } = "";

        public long? BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public string QueueNo { get; set; } = "";

        public string Ktpno { get; set; } = "";

        public string FamilyCardNo { get; set; } = "";

        public string Name { get; set; } = "";

        public DateTime? ExaminationDate { get; set; }

        public string Description { get; set; } = "";

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

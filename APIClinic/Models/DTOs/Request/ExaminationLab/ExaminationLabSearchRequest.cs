namespace APIClinic.Models.DTOs.Request
{
    public class ExaminationLabSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public string? QueueNo { get; set; } = "";

        public DateTime ExaminationDateFrom { get; set; }

        public DateTime ExaminationDateTo { get; set; }
    }
}

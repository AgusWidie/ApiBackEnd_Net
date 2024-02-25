namespace APIClinic.Models.DTOs.Request
{
    public class TransactionHeaderPatientLabRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public string? TransactionNo { get; set; } = "";

        public DateTime? TransactionDate { get; set; }

        public long? ExaminationLabId { get; set; }

        public string PaymentType { get; set; } = "";

        public string? Bpjsno { get; set; } = "";

        public string? InsuranceNo { get; set; } = "";

        public string? InsuranceName { get; set; } = "";

        public long? Total { get; set; }

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

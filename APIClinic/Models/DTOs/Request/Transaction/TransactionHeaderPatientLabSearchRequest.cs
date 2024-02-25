namespace APIClinic.Models.DTOs.Request
{
    public class TransactionHeaderPatientLabSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public DateTime? TransactionDateFrom { get; set; }

        public DateTime? TransactionDateTo { get; set; }

        public string? PaymentType { get; set; } = "";
    }
}

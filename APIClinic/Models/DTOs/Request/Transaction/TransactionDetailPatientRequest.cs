namespace APIClinic.Models.DTOs.Request
{
    public class TransactionDetailPatientRequest
    {
        public long? Id { get; set; }

        public string? TransactionNo { get; set; } = "";

        public long DrugId { get; set; }

        public string? UnitType { get; set; } = "";

        public long? Qty { get; set; }

        public long? Price { get; set; }

        public long? Subtotal { get; set; }

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

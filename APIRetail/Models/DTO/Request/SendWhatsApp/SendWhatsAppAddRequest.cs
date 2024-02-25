namespace APIRetail.Models.DTO.Request
{
    public class SendWhatsAppAddRequest
    {
        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? CustomerId { get; set; }

        public long? ScheduleId { get; set; }

        public long? MessageId { get; set; }

        public ulong? Active { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}

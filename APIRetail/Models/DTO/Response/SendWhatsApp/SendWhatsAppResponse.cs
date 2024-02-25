namespace APIRetail.Models.DTO.Response
{
    public class SendWhatsAppResponse
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public string CompanyName { get; set; } = "";

        public long? BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public long? CustomerId { get; set; }

        public string CustomerName { get; set; } = "";

        public long? ScheduleId { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public long? MessageId { get; set; }

        public string MessageData { get; set; } = "";

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

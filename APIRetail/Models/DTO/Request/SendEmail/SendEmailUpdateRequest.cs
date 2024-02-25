namespace APIRetail.Models.DTO.Request
{
    public class SendEmailUpdateRequest
    {
        public long? Id { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? CustomerId { get; set; }

        public long? ScheduleId { get; set; }

        public long? MessageId { get; set; }

        public ulong? Active { get; set; }

        public ulong? Send { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}

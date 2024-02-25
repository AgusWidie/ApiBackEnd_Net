namespace APIRetail.Models.DTO.Request
{
    public class MessageAddRequest
    {
        public long? CompanyId { get; set; }

        public string MessageData { get; set; }

        public ulong? Active { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}

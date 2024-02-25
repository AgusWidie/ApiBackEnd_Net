namespace APIRetail.Models.DTO.Request
{
    public class MessageUpdateRequest
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public string MessageData { get; set; } = "";

        public ulong? Active { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

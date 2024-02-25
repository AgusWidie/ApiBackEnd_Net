namespace APIRetail.Models.DTO.Request
{
    public class MessageRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? CompanyId { get; set; }
    }
}

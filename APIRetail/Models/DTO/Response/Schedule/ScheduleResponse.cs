namespace APIRetail.Models.DTO.Response
{
    public class ScheduleResponse
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public string CompanyName { get; set; } = "";

        public DateTime? ScheduleDate { get; set; }

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

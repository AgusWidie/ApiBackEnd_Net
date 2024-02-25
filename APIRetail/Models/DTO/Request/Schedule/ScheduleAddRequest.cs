namespace APIRetail.Models.DTO.Request
{
    public class ScheduleAddRequest
    {
        public long? CompanyId { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public ulong? Active { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}

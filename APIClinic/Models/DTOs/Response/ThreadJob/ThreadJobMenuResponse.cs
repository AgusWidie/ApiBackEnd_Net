namespace APIClinic.Models.DTOs.Response.ThreadJob
{
    public class ThreadJobMenuResponse
    {
        public string? JobName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? ListName { get; set; }
        public string? Description { get; set; }
    }
}

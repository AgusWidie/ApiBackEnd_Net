namespace APIClinic.Models.DTOs.Response
{
    public class ThreadJobTransactionResponse
    {
        public string? JobName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
    }
}

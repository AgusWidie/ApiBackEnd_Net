namespace APIClinic.Models.DTOs.Request
{
    public class LogErrorRequest
    {
        public string ServiceName { get; set; }

        public string ErrorDeskripsi { get; set; }

        public DateTime? ErrorDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}

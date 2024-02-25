namespace APIRetail.Models.DTO.Request
{
    public class CustomerAddRequest
    {

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Telephone { get; set; }

        public string? WhatsApp { get; set; }

        public string? Email { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}

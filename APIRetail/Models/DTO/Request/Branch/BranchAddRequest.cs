namespace APIRetail.Models.DTO.Request
{
    public class BranchAddRequest
    {
        public long? CompanyId { get; set; }

        public string Name { get; set; } = "";

        public string Address { get; set; } = "";

        public string Telp { get; set; } = "";

        public string Fax { get; set; } = "";

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

    }
}

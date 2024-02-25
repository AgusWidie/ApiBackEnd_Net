namespace APIRetail.Models.DTO.Response
{
    public class ProfilResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

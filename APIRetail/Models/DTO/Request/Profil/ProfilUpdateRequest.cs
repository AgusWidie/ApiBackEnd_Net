namespace APIRetail.Models.DTO.Request
{
    public class ProfilUpdateRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ulong? Active { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}

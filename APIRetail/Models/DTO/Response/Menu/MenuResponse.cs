namespace APIRetail.Models.DTO.Response
{
    public class MenuResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = "";

        public string ControllerName { get; set; } = "";

        public string ActionName { get; set; } = "";

        public string Description { get; set; } = "";

        public ulong? IsHeader { get; set; }

        public string? Icon { get; set; } = "";

        public ulong? Active { get; set; }

        public int? Sort { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

namespace APIRetail.Models.DTO.Request
{
    public class ProfilMenuRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? ProfilId { get; set; }

        public long? ParentMenuId { get; set; }
    }
}

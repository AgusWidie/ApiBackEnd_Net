namespace APIClinic.Models.DTOs.Request
{
    public class ProfilMenuSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? ProfilId { get; set; }

        public long? ParentMenuId { get; set; }
    }
}

namespace APIRetail.Models.DTO.Request
{
    public class UsersRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public string UserName { get; set; }
    }
}

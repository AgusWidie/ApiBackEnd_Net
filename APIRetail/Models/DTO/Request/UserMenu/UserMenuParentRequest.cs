namespace APIRetail.Models.DTO.Request
{
    public class UserMenuParentRequest
    {
        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public string? UserName { get; set; }
    }
}

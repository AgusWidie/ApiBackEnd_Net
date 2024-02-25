using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Models.DTO.Response
{
    public class MessageResponse
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public string CompanyName { get; set; } = "";

        public string MessageData { get; set; } = "";

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}

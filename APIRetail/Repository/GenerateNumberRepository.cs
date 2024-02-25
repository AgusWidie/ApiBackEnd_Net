using APIRetail.Models.Database;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class GenerateNumberRepository : IGenerateNumber
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public GenerateNumberRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<string> GeneratePurchaseNo(CancellationToken cancellationToken)
        {
            try
            {
                string PurchaseNo = "";
                long No = 0;

                var CheckPurchase = await _context.PurchaseOrderHeader.Where(x => x.PurchaseDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).OrderByDescending(x => x.PurchaseNo).FirstOrDefaultAsync();
                if (CheckPurchase != null)
                {
                    No = Convert.ToInt64(CheckPurchase.PurchaseNo.Substring(3, 10)) + 1;
                    PurchaseNo = CheckPurchase.PurchaseNo.Substring(0, 3) + No.ToString();

                }
                else
                {

                    PurchaseNo = "PRC" + DateTime.Now.ToString("yyyyMMdd") + "0001";
                }

                return PurchaseNo;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<string> GenerateInvoiceNo(CancellationToken cancellationToken)
        {
            try
            {
                string InvoiceNo = "";
                long No = 0;

                var CheckOrder = await _context.SalesOrderHeader.Where(x => x.SalesOrderDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).OrderByDescending(x => x.InvoiceNo).FirstOrDefaultAsync();
                if (CheckOrder != null)
                {
                    No = Convert.ToInt64(CheckOrder.InvoiceNo.Substring(3, 10)) + 1;
                    InvoiceNo = CheckOrder.InvoiceNo.Substring(0, 3) + No.ToString();

                }
                else
                {

                    InvoiceNo = "INV" + DateTime.Now.ToString("yyyyMMdd") + "0001";
                }

                return InvoiceNo;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}

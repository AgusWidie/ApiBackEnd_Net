namespace APIRetail.Repository.IRepository
{
    public interface IGenerateNumber
    {
        Task<string> GeneratePurchaseNo(CancellationToken cancellationToken = default);

        Task<string> GenerateInvoiceNo(CancellationToken cancellationToken = default);
    }
}

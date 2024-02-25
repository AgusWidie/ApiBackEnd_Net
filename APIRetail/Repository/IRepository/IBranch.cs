using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IBranch
    {
        Task<IEnumerable<BranchResponse>> GetBranch(BranchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<BranchResponse>> CreateBranch(BranchAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<BranchResponse>> UpdateBranch(BranchUpdateRequest param, CancellationToken cancellationToken = default);
    }
}

using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IBranchService
    {
        Task<List<BranchResponse>> GetBranch(BranchRequest param, CancellationToken cancellationToken);
        Task<List<BranchResponse>> CreateBranch(BranchAddRequest param, CancellationToken cancellationToken);
        Task<List<BranchResponse>> UpdateBranch(BranchUpdateRequest param, CancellationToken cancellationToken);
    }
}

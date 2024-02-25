using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IBranch
    {
        Task<IEnumerable<BranchResponse>> GetBranch(BranchSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<BranchResponse>> CreateBranch(BranchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<BranchResponse>> UpdateBranch(BranchRequest param, CancellationToken cancellationToken = default);
    }
}

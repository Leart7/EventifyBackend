using EventifyCommon.Models;

namespace EventifyWebApi.Repositories
{
    public interface IClosedAccount
    {
        Task<List<ClosedAccountReason>> GetClosedAccountReasons();
        Task<ClosedAccount> Create(ClosedAccount closedAccount);
        Task<ClosedAccountReason?> GetClosedAccountReason(int id);
    }
}

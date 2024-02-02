using EventifyCommon.Models;

namespace AuthApi.Repositories
{
    public interface IDeleteFollowings
    {
        Task RemoveFollowings(string userId);
    }
}

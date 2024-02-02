using EventifyCommon.Models;

namespace EventifyWebApi.Repositories
{
    public interface IFollowRepository
    {
        Task<List<Follow>> GetFollows(string userId);
        Task<List<(ApplicationUser, int)>> SuggestOrganizers(string? userId);
        Task<int> GetTotalNumberOfFollowers(string userId);
        Task<Follow?> GetFollow(int id);
        Task<Follow> CreateFollow(Follow follow, string userId);
        Task<Follow?> DeleteFollow(int id);
    }
}

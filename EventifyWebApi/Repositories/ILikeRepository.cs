using EventifyCommon.Models;
using EventifyCommon.Models.AbstractModels;

namespace EventifyWebApi.Repositories
{
    public interface ILikeRepository
    {
        Task<List<Like>> GetAll(string userId);
        Task<Like?> GetLike(int id);
        Task<Like> Create(Like like, string userId);
        Task<Like?> Delete(int id);
    }
}

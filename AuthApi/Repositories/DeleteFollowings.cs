using EventifyCommon.Context;
using EventifyCommon.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Repositories
{
    public class DeleteFollowings : IDeleteFollowings
    {
        private readonly ApplicationDbContext _db;

        public DeleteFollowings(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task RemoveFollowings(string userId)
        {
            var followerFollows = _db.Follows.Where(f => f.FollowerId == userId);
            _db.Follows.RemoveRange(followerFollows);

            await _db.SaveChangesAsync();
        }
    }
}

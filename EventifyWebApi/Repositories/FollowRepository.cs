using EventifyCommon.Context;
using EventifyCommon.Models;
using Microsoft.EntityFrameworkCore;

namespace EventifyWebApi.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly ApplicationDbContext _db;

        public FollowRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Follow> CreateFollow(Follow follow, string userId)
        {
            follow.FollowerId = userId;

            await _db.AddAsync(follow);
            await _db.SaveChangesAsync();
            return follow;
        }

        public async Task<Follow?> DeleteFollow(int id)
        {
            var follow = await _db.Follows.Include(f => f.FollowedUser).FirstOrDefaultAsync(f => f.Id == id);
            if (follow == null)
            {
                return null;
            }

            _db.Follows.Remove(follow);
            await _db.SaveChangesAsync();
            return follow;
        }

        public async Task<Follow?> GetFollow(int id)
        {
            var follow = await _db.Follows.FirstOrDefaultAsync(f => f.Id == id);
            if(follow == null)
            {
                return null;
            }

            return follow;
        }

        public async Task<List<Follow>> GetFollows(string userId)
        {
            return await _db.Follows.Include(f => f.FollowedUser).Where(f => f.FollowerId == userId).ToListAsync();
        }

        public async Task<int> GetTotalNumberOfFollowers(string userId)
        {
            var numberOfFollowers = await _db.Users
                .Where(u => u.Id == userId)
                .Select(u => u.FollowedUserFollows.Count)
                .FirstOrDefaultAsync();

            return numberOfFollowers;
        }

        public async Task<List<(ApplicationUser, int)>> SuggestOrganizers(string? currentUserId)
        {
            var suggestedUsers = await _db.Users
                .Where(u => u.Id != currentUserId) 
                .Where(u => !_db.Follows.Any(f => f.FollowerId == currentUserId && f.FollowedUserId == u.Id)) 
                .Where(u => _db.Events.Any(e => e.UserId == u.Id)) 
                .Select(u => new
                {
                    User = u,
                    FollowersCount = u.FollowedUserFollows.Count
                })
                .OrderByDescending(u => u.FollowersCount) 
                .Take(20) 
                .ToListAsync();

            return suggestedUsers.Select(u => (u.User, u.FollowersCount)).ToList();
        }

    }
}

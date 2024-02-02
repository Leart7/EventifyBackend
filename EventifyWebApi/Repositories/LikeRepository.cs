using EventifyCommon.Context;
using EventifyCommon.Models;
using EventifyCommon.Models.AbstractModels;
using Microsoft.EntityFrameworkCore;

namespace EventifyWebApi.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _db;

        public LikeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Like> Create(Like like, string userId)
        {
            like.UserId = userId;

            await _db.Likes.AddAsync(like);
            await _db.SaveChangesAsync();
            return like;
        }

        public async Task<Like?> Delete(int id)
        {
            var likedEvent = await _db.Likes.FirstOrDefaultAsync(l => l.Id == id);

            if(likedEvent == null)
            {
                return null;
            }

            _db.Remove(likedEvent);
            await _db.SaveChangesAsync();
            return likedEvent;
        }

        public async Task<List<Like>> GetAll(string userId)
        {
            return await _db.Likes.Include(l => l.Event).ThenInclude(e => e.Images).Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<Like?> GetLike(int id)
        {
            var likedEvent =  await _db.Likes.FirstOrDefaultAsync(l =>l.Id == id);
            if(likedEvent == null)
            {
                return null;
            }

            return likedEvent;
        }
    }
}

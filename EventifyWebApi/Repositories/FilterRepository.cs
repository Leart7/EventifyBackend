using EventifyCommon.Context;
using EventifyCommon.Models.AbstractModels;
using Microsoft.EntityFrameworkCore;

namespace EventifyWebApi.Repositories
{
    public class FilterRepository<T> : IFilterRepository<T> where T : FilterModel
    {
        private readonly ApplicationDbContext _db;

        public FilterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<T>> GetAll()
        {
            return await _db.Set<T>().ToListAsync();
        }
    }
}

using EventifyCommon.Context;
using EventifyCommon.Models.AbstractModels;
using Microsoft.EntityFrameworkCore;
using static EventifyMVC.Enums.Enums;

namespace EventifyMVC.Repositories
{
    public class FilterRepository<T> : IFilterRepository<T> where T : FilterModel
    {
        private readonly ApplicationDbContext _db;


        public FilterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<T> Create(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Delete(int id)
        {
            var filter = await _db.Set<T>().FirstOrDefaultAsync(f => f.Id == id);
            if(filter == null)
            {
                return null;
            }

            _db.Set<T>().Remove(filter);
            await _db.SaveChangesAsync();
            return filter;
        }

        public async Task<T> FindFilter(int id)
        {
            var filter = await _db.Set<T>().Include(f => f.Events).FirstOrDefaultAsync(f => f.Id == id);
            if (filter == null)
            {
                return null;
            }

            return filter;
        }

        public async Task<(List<T> Filters, int TotalPages, int TotalResults)> GetAll(string? name = null, string? orderBy = null, int pageNumber = 1)
        {
            var filters = _db.Set<T>().AsQueryable();
            if(!string.IsNullOrEmpty(name))
            {
                filters = filters.Where(filter => filter.Name.Contains(name));
            }

            switch (orderBy)
            {
                case "name_dsc":
                    filters = filters.OrderByDescending(f => f.Name);
                    break;
                case "name_asc":
                    filters = filters.OrderBy(f => f.Name);
                    break;
            }

            int totalRecords = await filters.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)PageSize.Default);
            if (totalPages == 0)
            {
                totalPages = 1;
            }


            var skipResults = (pageNumber - 1) * (int)PageSize.Default;
            var resultFilters = await filters.Skip(skipResults).Take((int)PageSize.Default).ToListAsync();


            var returnedObj = new
            {
                Filters = resultFilters,
                TotalPages = totalPages
            };

            return (resultFilters, totalPages, totalRecords);
        }

        public async Task<T?> Update(int id, T entity)
        {
            var existingEntity = await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            if (existingEntity == null)
            {
                return null;
            }

            existingEntity.Name = entity.Name;
            existingEntity.Updated_at = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return entity;
        }


        public async Task<(T? MostTrendingFilter, int NumberOfEvents)> GetMostTrendingFilter()
        {
            var result = await _db.Set<T>()
                .Select(filter => new
                {
                    Filter = filter,
                    NumberOfEvents = filter.Events.Count
                })
                .OrderByDescending(x => x.NumberOfEvents)
                .FirstOrDefaultAsync();

            return (result?.Filter, result?.NumberOfEvents ?? 0);
        }

        public async Task<(T? MostTrendingFilter, int NumberOfEvents)> GetMostUnusedFilter()
        {
            var result = await _db.Set<T>()
                .Select(filter => new
                {
                    Filter = filter,
                    NumberOfEvents = filter.Events.Count
                })
                .OrderBy(x => x.NumberOfEvents)
                .FirstOrDefaultAsync();

            return (result?.Filter, result?.NumberOfEvents ?? 0);
        }


    }
}

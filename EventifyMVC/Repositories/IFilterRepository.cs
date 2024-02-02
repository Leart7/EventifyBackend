using EventifyCommon.Models;
using EventifyCommon.Models.AbstractModels;
namespace EventifyMVC.Repositories
{
    public interface IFilterRepository<T> where T : FilterModel
    {
        Task<(List<T> Filters, int TotalPages, int TotalResults)> GetAll(string? name = null, string? orderBy = null, int pageNumber = 1);
        Task<(T? MostTrendingFilter, int NumberOfEvents)> GetMostTrendingFilter();
        Task<(T? MostTrendingFilter, int NumberOfEvents)> GetMostUnusedFilter();
        Task<T> Create(T entity);
        Task<T> FindFilter(int id);
        Task<T?> Update(int id, T entity);
        Task<T?> Delete(int id);
    }
}

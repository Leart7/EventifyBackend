using EventifyCommon.Models.AbstractModels;
namespace EventifyWebApi.Repositories
{
    public interface IFilterRepository<T> where T : FilterModel
    {
        Task<List<T>> GetAll();
    }
}

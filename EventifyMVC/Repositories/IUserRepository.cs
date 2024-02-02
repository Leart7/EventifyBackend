using EventifyCommon.Models;
using Microsoft.AspNetCore.Identity;

namespace EventifyMVC.Repositories
{
    public interface IUserRepository
    {
        Task<(List<IdentityUser> Users, int TotalPages, int TotalResults)> GetAll(string? term = null, string? orderBy = null, int pageNumber = 1);
        Task<IdentityUser?> FindUser(string userId);
        Task<(List<Event> Events, int TotalPages, int TotalResults)> GetEvents(string userId, string? title = null, string? category = null, string? orderBy = "date_soonest_first", string? status = null, string? pastEvents = "false", string? online = null, int pageNumber = 1);
    }
}

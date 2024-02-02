using EventifyCommon.Context;
using EventifyCommon.Models;
using EventifyMVC.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static EventifyMVC.Enums.Enums;

namespace EventifyMVC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IdentityUser?> FindUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<(List<IdentityUser> Users, int TotalPages, int TotalResults)> GetAll(string? term = null, string? orderBy = null, int pageNumber = 1)
        {
            var usersQueryable = _userManager.Users
                .OfType<ApplicationUser>()
                .Include(u => u.Events)
                    .ThenInclude(e => e.Reports)
                .Include(u => u.FollowedUserFollows)
                .AsQueryable();

            var allUsers = await usersQueryable.ToListAsync();

            var userIds = await _userManager.GetUsersInRoleAsync(Roles.Role_User);

            var users = allUsers.Where(u => userIds.Any(adminUser => adminUser.Id == u.Id));

            if (!string.IsNullOrEmpty(term))
            {
                users = users.Where(u =>
                    u.Email.Contains(term) ||
                    u.FirstName.Contains(term) ||
                    u.LastName.Contains(term) ||
                    u.Id == term);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "email_dsc":
                        users = users.OrderByDescending(u => u.UserName);
                        break;
                    case "email_asc":
                        users = users.OrderBy(u => u.UserName);
                        break;
                    case "name_dsc":
                        users = users.OrderByDescending(u => u.FirstName);
                        break;
                    case "name_asc":
                        users = users.OrderBy(u => u.FirstName);
                        break;
                    case "lastname_dsc":
                        users = users.OrderByDescending(u => u.LastName);
                        break;
                    case "lastname_asc":
                        users = users.OrderBy(u => u.LastName);
                        break;
                    case "events_dsc":
                        users = users.OrderByDescending(u => u.Events.Count);
                        break;
                    case "events_asc":
                        users = users.OrderBy(u => u.Events.Count);
                        break;
                    case "followers_dsc":
                        users = users.OrderByDescending(u => u.FollowedUserFollows.Count);
                        break;
                    case "followers_asc":
                        users = users.OrderBy(u => u.FollowedUserFollows.Count);
                        break;
                    case "reports_dsc":
                        users = users.OrderByDescending(u => u.Events.SelectMany(e => e.Reports).Count());
                        break;
                    case "reports_asc":
                        users = users.OrderBy(u => u.Events.SelectMany(e => e.Reports).Count());
                        break;

                }
            }

            var totalResults = users.Count();

            var pageSize = PageSize.Default;
            var totalPages = (int)Math.Ceiling(totalResults / (double)pageSize);

            var skipResults = (pageNumber - 1) * (int)PageSize.Default;
            var resultUsers = users.Skip(skipResults).Take((int)pageSize).ToList();

            var identityUsers = resultUsers.OfType<IdentityUser>().ToList();

            return (identityUsers, totalPages, totalResults);
        }

        public async Task<(List<Event> Events, int TotalPages, int TotalResults)> GetEvents(string userId, string? title = null, string? category = null, string? orderBy = "date_soonest_first", string? status = null, string? pastEvents = "false", string? online = null, int pageNumber = 1)
        {
            var events = _db.Events.Where(e => e.UserId == userId).Include(e => e.Category).Include(e => e.Language).Include(e => e.Format).Include(e => e.Currency).Include(e => e.Type).Include(e => e.Images).Include(e => e.User).AsQueryable();

            if (!String.IsNullOrEmpty(title))
            {
                events = events.Where(e => e.Title.ToLower().Contains(title.ToLower()));
            }

            if (!String.IsNullOrEmpty(category))
            {
                events = events.Where(e => e.Category.Name == category);
            }

            if (!String.IsNullOrEmpty(status))
            {
                events = events.Where(e => e.Status == status);
            }

            if (online == "true")
            {
                events = events.Where(e => e.Latitude == null);
            }

            if (pastEvents == "true")
            {
                events = events.Where(e => e.StartTime < DateTime.UtcNow);
            }
            else if (pastEvents == "false")
            {
                events = events.Where(e => e.StartTime > DateTime.UtcNow);
            }


            switch (orderBy)
            {
                case "date_dsc":
                    events = events.OrderByDescending(e => e.Created_at);
                    break;
                case "date_asc":
                    events = events.OrderBy(e => e.Created_at);
                    break;
                case "date_soonest_first":
                    events = events.OrderBy(e => e.StartTime);
                    break;
            }


            int totalRecords = await events.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)PageSize.Default);
            if (totalPages == 0)
            {
                totalPages = 1;
            }


            var skipResults = (pageNumber - 1) * (int)PageSize.Default;
            var resultEvents = await events.Skip(skipResults).Take((int)PageSize.Default).ToListAsync();


            var returnedObj = new
            {
                Events = resultEvents,
                TotalPages = totalPages
            };

            return (resultEvents, totalPages, totalRecords);
        }
    }
}

using EventifyCommon.Context;
using EventifyCommon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static EventifyMVC.Enums.Enums;

namespace EventifyMVC.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _db;

        public ReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<(List<ReportEvent> Reports, int TotalPages, int TotalResults)> GetAll(string? term = null, string orderBy = "date_dsc", string reviewed = "false", int pageNumber = 1)
        {
            var reports = _db.ReportEvents.Include(r => r.ReportEventReason).Include(r => r.Event).ThenInclude(e => e.User).AsQueryable();

            if (!String.IsNullOrEmpty(term))
            {
                if (int.TryParse(term, out int termId))
                {
                    reports = reports.Where(r => r.Event.Id == termId);
                }
                else
                {
                    term = term.ToLower();
                    reports = reports.Where(r =>
                        r.Event.User.Id.ToLower() == term ||
                        r.Event.Title.ToLower().Contains(term) ||
                        r.Event.User.FirstName.ToLower().Contains(term)
                    );
                }
            }

            if (reviewed == "true")
            {
                reports = reports.Where(r => r.Reviewed == true);
            }
            else if (reviewed == "false")
            {
                reports = reports.Where(r => r.Reviewed == false);
            }

            switch (orderBy)
            {
                case "date_dsc":
                    reports = reports.OrderByDescending(r => r.Created_at);
                    break;
                case "date_asc":
                    reports = reports.OrderBy(r => r.Created_at);
                    break;
            }

            int totalRecords = await reports.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)PageSize.Default);
            if (totalPages == 0)
            {
                totalPages = 1;
            }


            var skipResults = (pageNumber - 1) * (int)PageSize.Default;
            var resultReports = await reports.Skip(skipResults).Take((int)PageSize.Default).ToListAsync();


            var returnedObj = new
            {
                Reports = resultReports,
                TotalPages = totalPages
            };

            return (resultReports, totalPages, totalRecords);
        }

        public async Task UpdateReportStatus(int id)
        {
            var report = await _db.ReportEvents.FirstOrDefaultAsync(r => r.Id == id);

            if (report != null)
            {
                report.Reviewed = true;
                await _db.SaveChangesAsync();
            }
        }

    }
}

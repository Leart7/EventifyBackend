using EventifyCommon.Context;
using EventifyCommon.Models;
using Microsoft.EntityFrameworkCore;

namespace EventifyWebApi.Repositories
{
    public class ReportEventRepository : IReportEventRepository
    {
        private readonly ApplicationDbContext _db;

        public ReportEventRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ReportEventReason?> GetReportEventReason(int id)
        {
            var reportReason = await _db.ReportEventReasons.FirstOrDefaultAsync(r => r.Id == id);

            if(reportReason == null)
            {
                return null;
            }

            return reportReason;
        }

        public async Task<List<ReportEventReason>> GetReportEventReasons()
        {
            return await _db.ReportEventReasons.ToListAsync();
        }

        public async Task<ReportEvent> ReportEvent(ReportEvent reportEvent)
        {
            await _db.ReportEvents.AddAsync(reportEvent);
            await _db.SaveChangesAsync();
            return reportEvent;
        }
    }
}

using EventifyCommon.Models;

namespace EventifyWebApi.Repositories
{
    public interface IReportEventRepository
    {
        Task<ReportEvent> ReportEvent(ReportEvent reportEvent);
        Task<List<ReportEventReason>> GetReportEventReasons();
        Task<ReportEventReason?> GetReportEventReason(int id);
    }
}

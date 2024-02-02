using EventifyCommon.Models;

namespace EventifyMVC.Repositories
{
    public interface IReportRepository
    {
        Task<(List<ReportEvent> Reports, int TotalPages, int TotalResults)> GetAll(string? term = null, string orderBy = "date_dsc", string reviewed = "false", int pageNumber = 1);
        Task UpdateReportStatus(int id);
    }
}

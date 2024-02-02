namespace EventifyWebApi.DTOs
{
    public class CreateReportEventRequestDto
    {
        public int ReportEventReasonId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int EventId { get; set; }
    }
}

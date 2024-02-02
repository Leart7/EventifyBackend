using AutoMapper;
using EventifyCommon.Models;
using EventifyWebApi.DTOs;
using EventifyWebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventifyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportEventController : ControllerBase
    {
        private readonly IReportEventRepository _reportEventRepository;
        private readonly IMapper _mapper;

        public ReportEventController(IReportEventRepository reportEventRepository, IMapper mapper)
        {
            _reportEventRepository = reportEventRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportEventReasons()
        {
            var reportReasons = await _reportEventRepository.GetReportEventReasons();
            return Ok(_mapper.Map<List<ReportEventReasonDto>>(reportReasons));
        }

        [HttpPost]
        public async Task<IActionResult> ReportEvent(CreateReportEventRequestDto createReportEventRequestDto)
        {
            var reportReason = await _reportEventRepository.GetReportEventReason(createReportEventRequestDto.ReportEventReasonId);

            if(reportReason == null)
            {
                return NotFound();
            }

            var reportDomainModel = _mapper.Map<ReportEvent>(createReportEventRequestDto);
            reportDomainModel = await _reportEventRepository.ReportEvent(reportDomainModel);

            return Ok(_mapper.Map<ReportEventDto>(reportDomainModel));
        }
    }
}

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
    public class LanguageController : ControllerBase
    {

        private readonly IFilterRepository<Language> _repository;
        private readonly IMapper _mapper;

        public LanguageController(IFilterRepository<Language> filterRepository, IMapper mapper)
        {
            _repository = filterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguages()
        {
            var languages = await _repository.GetAll();
            return Ok(_mapper.Map<List<LanguageDto>>(languages));
        }
    }
}

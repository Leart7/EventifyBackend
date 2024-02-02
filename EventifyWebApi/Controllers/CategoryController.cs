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
    
    public class CategoryController : ControllerBase
    {

        private readonly IFilterRepository<Category> _repository;
        private readonly IMapper _mapper;

        public CategoryController(IFilterRepository<Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _repository.GetAll();    
            return Ok(_mapper.Map<List<CategoryDto>>(categories));
        }
    }

}

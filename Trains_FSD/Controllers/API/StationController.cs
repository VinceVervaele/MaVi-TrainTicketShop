using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Trains_FSD.Models.Entities;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : Controller
    {
        private readonly IMapper _mapper;
        private IService<City> _cityService;

        public StationController(IMapper mapper, IService<City> cityService)
        {
            _mapper = mapper;
            _cityService = cityService;
        }

        [HttpGet("GetAllStations")] // /api/station/getallstations
        public async Task<IEnumerable<CityVM>> GetAll()
        {
            var cityList = await _cityService.GetAll();
            return _mapper.Map<List<CityVM>>(cityList);
        }
    }
}

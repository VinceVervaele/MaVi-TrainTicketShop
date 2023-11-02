using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Trains_FSD.Models.Entities;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class CityController : Controller
    {

        private readonly IMapper _mapper;
        private IService<City> _cityService;

        public CityController(IMapper mapper, IService<City> cityService)
        {
            _mapper = mapper;
            _cityService = cityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Cities()
        {
            var cityList = await _cityService.GetAll();
            List<CityVM> cityListVM = _mapper.Map<List<CityVM>>(cityList);
            return View(cityListVM);
        }

        [HttpPost]
        public async Task<IActionResult> Cities(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Ticket", new { id = id });
        }
    }
}

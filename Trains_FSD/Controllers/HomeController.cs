using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using Trains_FSD.Areas.Data;
using Trains_FSD.Extensions;
using Trains_FSD.Models;
using Trains_FSD.Models.Entities;
using Trains_FSD.Services;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private IServiceOrder<Order> _orderService;
        private IService<City> _cityService;
        private IServiceTraject<Traject> _trajectService;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _singInManager;

        public HomeController(IStringLocalizer<HomeController> localizer, IMapper mapper, IServiceTraject<Traject> trajectService, IService<City> cityService, 
            IServiceOrder<Order> orderService, SignInManager<User> singInManager)
        {
            _localizer = localizer;
            _orderService = orderService;
            _cityService = cityService;
            _trajectService = trajectService;
            _mapper = mapper;
            _singInManager = singInManager;
        }

        [HttpPost]
        public IActionResult SetAppLanguage(string lang, string returnUrl)
        {

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Index()
        {
            var userId = _singInManager.UserManager.GetUserId(User);
            List<string> cities = new List<string>();

            if (User.Identity.IsAuthenticated)
            {
                var orderList = await _orderService.FindByCustomerId(userId);
                if(orderList.Count() == 0) {
                    cities = _cityService.GetAll().Result.Select(c => c.Name).ToList();
                    ViewBag.Ordered = false;
                } else
                {
                    List<OrderVM> orderListVM = _mapper.Map<List<OrderVM>>(orderList);

                    foreach (var order in orderListVM)
                    {
                        foreach (var ticket in order.Tickets)
                        {
                                var traject = await _trajectService.FindById(ticket.TicketDetails.Last().TrajectId);
                                TrajectVM trajectVM = _mapper.Map<TrajectVM>(traject);
                                string arrivalCityName = _cityService.FindById(trajectVM.ArrivalCityId).Result.Name;
                                if (!cities.Contains(arrivalCityName))
                                {
                                    cities.Add(arrivalCityName);
                                }
                        }
                    }
                    ViewBag.Ordered = true;
                }
                
            } else
            {
                cities = _cityService.GetAll().Result.Select(c => c.Name).ToList();
                ViewBag.Ordered = false;
            }
            ViewBag.FavCities = cities;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Trains_FSD.Extensions;

using Trains_FSD.Models;

using Trains_FSD.Models.Entities;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class TicketController : Controller
    {
        private readonly IMapper _mapper;
        private IService<City> _cityService;
        private IService<Vacation> _vacationService;
        private IService<Train> _trainService;
        private IServiceOrder<Order> _orderService;
        private IService<Line> _lineService;
        private IServiceTicketDetails<TicketDetail> _ticketDetailService;
        private IServiceTraject<Traject> _trajectService;
        private IService<TrajectLine> _trajectLineService;

        public TicketController(IMapper mapper, IService<Vacation> vacationService, IService<Train> trainService, IServiceTicketDetails<TicketDetail> ticketDetailService,
            IService<City> cityService, IServiceOrder<Order> orderService, IService<Line> lineService,
            IServiceTraject<Traject> trajectService, IService<TrajectLine> trajectLineService)
        {
            _mapper = mapper;
            _cityService = cityService;
            _trainService = trainService;
            _vacationService = vacationService;
            _orderService = orderService;
            _lineService = lineService;
            _ticketDetailService = ticketDetailService;
            _trajectService = trajectService;
            _trajectLineService = trajectLineService;

        }

        public async Task<IActionResult> Index()
        {
            var ticket = await GetTicketVM();

            return RedirectToAction("TicketShop", ticket.DepartureCityId);
        }

        public async Task<IActionResult> TicketShop(int id)
        {
            var ticket = await GetTicketVM();

            ticket.ArrivalCityId = id;

            if (ticket.ArrivalCityId != null)
            {
                ticket.DepartureCity = ticket.DepartureCity ?? new SelectList(new List<City>(), "Id", "Name");
                foreach (SelectListItem item in ticket.DepartureCity)
                {
                    if (item.Value == ticket.ArrivalCityId.ToString())
                    {
                        item.Disabled = true;
                    }
                }
            }
            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> TicketShop(TicketVM entity)
        {

            entity.SearchResults = new List<SearchResultVM>();

            for (int i = 0; i < 7; i++)
            {
                DateTime searchDate = DateTime.Parse(entity.DepartureDate).AddDays(i); ;
                if (searchDate.Date == DateTime.Now.Date)
                {
                    searchDate = searchDate + DateTime.Now.TimeOfDay;
                }

                var traject = await _trajectService.FindByMultipleIds(entity.DepartureCityId, entity.ArrivalCityId);

                List<LineVM> listLineVMs = new List<LineVM>();
                foreach (var trajectLine in traject.TrajectLines)
                {
                    var lineVM = _mapper.Map<LineVM>(trajectLine.Line);
                    var smallTraject = await _trajectService.FindByMultipleIds(lineVM.DepartureCity.Id, lineVM.ArrivalCity.Id);
                    lineVM.TrajectId = smallTraject.Id;
                    listLineVMs.Add(lineVM);
                }

                listLineVMs.Sort(new LineComparator());

                SearchResultVM searchResult = new SearchResultVM
                {
                    Lines = listLineVMs,
                    DepartureCity = listLineVMs.First().DepartureCity.Name,
                    ArrivalCity = listLineVMs.Last().ArrivalCity.Name,
                    Class = entity.Class
                };

                searchResult.Lines.ForEach(l =>
                {
                    l.DepartureTime = searchDate.Date + l.DepartureTime.TimeOfDay;
                    l.ArrivalTime = searchDate.Date + l.ArrivalTime.TimeOfDay;
                });
                ValidateOverlappingTimes(searchResult.Lines);

                await ValidateSeats(searchResult);


                searchResult.Price = CalculatePrice(listLineVMs, entity.Class);

                searchResult.DepartureDate = searchDate.ToString("dd/MM/yyyy");

                AddSearchResult(searchResult, searchDate, entity.SearchResults, listLineVMs);
            }

            await FillAllCitySelectLists(entity);
            return View(entity);
        }

        private void ValidateOverlappingTimes(List<LineVM> lines)
        {
            var daysToAdd = 1;
            for (var i = 0; i < lines.Count - 1; i++)
            {
                if (lines[i].ArrivalTime > lines[i + 1].DepartureTime)
                {
                    lines[i + 1].DepartureTime = lines[i + 1].DepartureTime.AddDays(daysToAdd);
                    lines[i + 1].ArrivalTime = lines[i + 1].ArrivalTime.AddDays(daysToAdd);
                    daysToAdd++;
                }

            }
        }

        private async void ValidateVacation(SearchResultVM searchResultVM)
        {

            var startDate = searchResultVM.Lines.First().DepartureTime;
            var endDate = searchResultVM.Lines.Last().ArrivalTime;

            var vacationPeriods = await GetVacationPeriods(startDate, endDate);

            for (var i = 0; i < vacationPeriods.Count(); i++)
            {
                if (vacationPeriods.ElementAt(i).Id == 1 && IsArrivalCityInList(searchResultVM.Lines.Last().ArrivalCity.Name, new string[] { "Paris", "London" }))
                {
                    ModifyTrainCapacities(searchResultVM.Lines, vacationPeriods.ElementAt(i).ModifierPercentage);
                }
                else if (vacationPeriods.ElementAt(i).Id == 2 && IsArrivalCityInList(searchResultVM.Lines.Last().ArrivalCity.Name, new string[] { "Paris", "Brussels", "Amsterdam" }))
                {
                    ModifyTrainCapacities(searchResultVM.Lines, vacationPeriods.ElementAt(i).ModifierPercentage);
                }
            }
        }

        private async Task<IEnumerable<Vacation>> GetVacationPeriods(DateTime startDate, DateTime endDate)
        {
            var vacations = await _vacationService.GetAll();
            return vacations.Where(v => v.StartDate <= endDate && v.EndDate >= startDate).ToArray();
        }

        private bool IsArrivalCityInList(string cityName, string[] cityList)
        {
            return cityList.Contains(cityName);
        }

        private void ModifyTrainCapacities(List<LineVM> lines, double modifierPercentage)
        {
            foreach (var line in lines)
            {
                line.Train.CapacityEconomyClass += (int)Math.Ceiling(line.Train.CapacityEconomyClass * (modifierPercentage / 100));
                line.Train.CapacityBusinessClass += (int)Math.Ceiling(line.Train.CapacityBusinessClass * (modifierPercentage / 100));
            }
        }

        private async Task ValidateSeats(SearchResultVM searchResultVM)
        {
            ValidateVacation(searchResultVM);

            foreach (var lineVM in searchResultVM.Lines)
            {
                
                int capacity = searchResultVM.Class == "business" ? lineVM.Train.CapacityBusinessClass : lineVM.Train.CapacityEconomyClass;
                var tickets = searchResultVM.Class == "business" ? await _ticketDetailService.FindTicketsByDeparture(lineVM.DepartureTime.Date, lineVM.TrajectId, searchResultVM.Class)
                : await _ticketDetailService.FindTicketsByDeparture(lineVM.DepartureTime.Date, lineVM.TrajectId, searchResultVM.Class);
                int bookings = tickets.Count();

                if (bookings >= capacity)
                {
                    searchResultVM.isValid = false;
                    break;
                }
            }
        }

        private double CalculatePrice(ICollection<LineVM> lines, string classType)
        {
            var price = 0.0;
            foreach (var line in lines)
            {
                if (classType.Equals("business"))
                {
                    price += line.PriceBusinessClass;
                }
                else
                {
                    price += line.PriceEconomyClass;
                }
            }
            return price;
        }

        private void AddSearchResult(SearchResultVM searchResult, DateTime searchDate, List<SearchResultVM> searchResults, List<LineVM> orderedLines)
        {

            if (searchDate.Date == DateTime.Now.Date)
            {
                if (DateTime.Now.TimeOfDay < orderedLines.First().DepartureTime.TimeOfDay)
                {
                    searchResults.Add(searchResult);
                }
                else
                {
                    searchResult.isValid = false;
                    searchResults.Add(searchResult);
                }
            }
            else if (searchDate >= DateTime.Now.AddDays(14))
            {
                searchResult.isValid = false;
                searchResults.Add(searchResult);
            }
            else
            {
                searchResults.Add(searchResult);
            }

        }
        private async Task FillAllCitySelectLists(TicketVM entity)
        {
            var cities = await _cityService.GetAll();

            var departureCitySelectList = new SelectList(cities, "Id", "Name", entity.ArrivalCityId);
            var arrivalCitySelectList = new SelectList(cities, "Id", "Name", entity.DepartureCityId);

            entity.DepartureCity = departureCitySelectList;
            entity.ArrivalCity = arrivalCitySelectList;
        }

        private async Task<TicketVM> GetTicketVM()
        {
            return new TicketVM()
            {
                DepartureCity = new SelectList(await _cityService.GetAll(), "Id", "Name"),
                ArrivalCity = new SelectList(await _cityService.GetAll(), "Id", "Name")
            };
        }

        public async Task<IActionResult> Select(String modelJson)
        {
            string json = HttpContext.Request.Query["modelJson"];
            var entity = JsonConvert.DeserializeObject<SearchResultVM>(json);

            if (entity == null)
            {
                return NotFound();

            }

            ShoppingCartVM shoppingCartVM;

            if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
            {
                shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            }
            else
            {
                shoppingCartVM = new ShoppingCartVM();
                shoppingCartVM.Cart = new List<CartVM>();
            }

            CartVM cart = new CartVM
            {
                Cartnr = shoppingCartVM.Cart.Count,
                Amount = 1,
                SearchResultVM = entity,
                CartPrice = entity.Price
            };
            bool alreadyInCart = false;
            int i = 0;
            while (!alreadyInCart && shoppingCartVM.Cart.Count > i)
            {
                if (shoppingCartVM.Cart[i].SearchResultVM.Equals(cart.SearchResultVM))
                {
                    shoppingCartVM.TotalPrice += shoppingCartVM.Cart[i].CartPrice;
                    shoppingCartVM.Cart[i].Amount = shoppingCartVM.Cart[i].Amount + 1;
                    alreadyInCart = true;
                }
                i++;
            }
            if (!alreadyInCart)
            {
                shoppingCartVM?.Cart?.Add(cart);
                shoppingCartVM.TotalPrice += cart.CartPrice;
            }

            HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}

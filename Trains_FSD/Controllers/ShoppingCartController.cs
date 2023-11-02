using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Trains_FSD.Areas.Data;
using Trains_FSD.Extensions;
using Trains_FSD.Models.Entities;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.Util.Mail;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IEmailSend _sender;

        private readonly IMapper _mapper;
        private SignInManager<User> _signInManager;
        private IServiceOrder<Order> _orderService;
        private IServiceTraject<Traject> _trajectService;
        private IService<Ticket> _ticketService;
        private IServiceTicketDetails<TicketDetail> _ticketDetailService;

        public ShoppingCartController(ILogger<HomeController> logger, IEmailSend sender, SignInManager<User> signInManager,
            IServiceOrder<Order> orderService, IMapper mapper, IServiceTraject<Traject> trajectService, IService<Ticket> ticketService
            , IServiceTicketDetails<TicketDetail> ticketDetailService)
        {
            _logger = logger;
            _sender = sender;
            _signInManager = signInManager;
            _orderService = orderService;
            _mapper = mapper;
            _trajectService = trajectService;
            _ticketService = ticketService;
            _ticketDetailService = ticketDetailService;
        }

        public IActionResult Index()
        {
            string hotelName = TempData["HotelName"] as string;
            if (hotelName != null)
            {
                ViewBag.HotelName = hotelName;
            }

            ShoppingCartVM shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shopping == null)
            {
                return View();
            }
            else
            {
                return View(shopping);
            }
        }

        public IActionResult Delete(int id)
        {
            var cart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            var itemToRemove = cart.Cart.FirstOrDefault(r => r.Cartnr == id);
            cart.TotalPrice -= itemToRemove.Amount * itemToRemove.CartPrice;
            if (itemToRemove != null)
            {
                cart.Cart.Remove(itemToRemove);
                HttpContext.Session.SetObject("ShoppingCart", cart);
            }

            return RedirectToAction("Index");

        }

        public void sendEmail(string user)
        {
            var userEmail = _signInManager.UserManager.FindByIdAsync(user).Result.Email;

            string message = "Dear " + User.Identity.Name + "<br />  <br />" +
                "Thank you for placing an order with us! " +
                "Please note that your order is not yet confirmed and payment has not been processed. <br />" +
                "Don't forget to confirm your order." +
                "Thank you again for your order, we look forward to providing you with the best service possible.  <br /><br />" +
                "Kind regards,  <br />" +
                "MaVi Trains";

            _sender.SendEmailAsync(userEmail, "Train E-mail confirmation", message);
        }

        [Authorize]
        public async Task<IActionResult> Order()
        {
            ShoppingCartVM shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            if (await IsTrajectAvailable(shopping))
            {

                var user = _signInManager.UserManager.GetUserId(User);

                sendEmail(user);

                OrderVM orderVM = new OrderVM()
                {
                    OrderDate = DateTime.Now,
                    CustomerId = user,
                    Confirmed = false
                };

                var order = _mapper.Map<Order>(orderVM);
                await _orderService.Add(order);

                var orderLast = await _orderService.GetAll();
                int id = orderLast.Last().Id;

                foreach (CartVM cartVM in shopping.Cart)
                {
                    cartVM.Available = true;
                    for (int i = 0; i < cartVM.Amount; i++)
                    {

                        TicketAddVM ticketVM = new TicketAddVM()
                        {
                            OrderId = id
                        };

                        var ticket = _mapper.Map<Ticket>(ticketVM);
                        await _ticketService.Add(ticket);

                        var ticketLast = await _ticketService.GetAll();
                        int idTicket = ticketLast.Last().Id;

                        foreach (LineVM lineVM in cartVM.SearchResultVM.Lines)
                        {
                            var traject = await _trajectService.FindByMultipleIds(lineVM.DepartureCity.Id, lineVM.ArrivalCity.Id);
                            var freeSeat = await AssignPlace(lineVM, cartVM, lineVM.TrajectId);

                            TicketDetailVM ticketDetailVM = new TicketDetailVM()
                            {
                                Seat = freeSeat,
                                Type = cartVM.SearchResultVM.Class,
                                TrajectId = traject.Id,
                                TicketId = idTicket,
                                DepartureDate = lineVM.DepartureTime
                            };

                            var ticketDetail = _mapper.Map<TicketDetail>(ticketDetailVM);
                            await _ticketDetailService.Add(ticketDetail);
                        }
                    }
                }


                HttpContext.Session.SetObject("ShoppingCart", null);

                return RedirectToAction("GetPersonalOrders", "Order");
            }
            return RedirectToAction("Index");

        }

        private async Task<bool> IsTrajectAvailable(ShoppingCartVM shopping)
        {
            bool result = true;
            foreach (CartVM cartVM in shopping.Cart)
            {
                foreach (LineVM lineVM in cartVM.SearchResultVM.Lines)
                {
                    var usedPlaces = await _ticketDetailService.FindTicketsByDeparture(lineVM.DepartureTime.Date, lineVM.TrajectId, cartVM.SearchResultVM.Class);
                    int availableCapacity = cartVM.SearchResultVM.Class == "business" ? lineVM.Train.CapacityBusinessClass : lineVM.Train.CapacityEconomyClass;

                    if (usedPlaces.Count() + cartVM.Amount > availableCapacity)
                    {
                        cartVM.Available = false;
                        result = false;
                    }
                }
            }
            HttpContext.Session.SetObject("ShoppingCart", shopping);
            return result;
        }

        private async Task<string> AssignPlace(LineVM lineVM, CartVM cartVM, int trajectId)
        {
            if (cartVM.SearchResultVM.Class == "business")
            {
                var tickets = await _ticketDetailService.FindTicketsByDeparture(lineVM.DepartureTime.Date, trajectId, cartVM.SearchResultVM.Class);
                if (tickets.Count() == 0)
                {
                    return "B1";
                }
                else
                {
                    for (int j = 1; j <= lineVM.Train.CapacityBusinessClass; j++)
                    {
                        var seat = "B" + j;
                        if (!tickets.Any(t => t.Seat == seat))
                        {
                            return seat;
                        }
                    }
                }
            }
            else
            {
                var tickets = await _ticketDetailService.FindTicketsByDeparture(lineVM.DepartureTime.Date, lineVM.TrajectId, cartVM.SearchResultVM.Class);
                if (tickets.Count() == 0)
                {
                    return "E1";
                }
                else
                {
                    for (int j = 1; j <= lineVM.Train.CapacityEconomyClass; j++)
                    {
                        var seat = "E" + j;
                        if (!tickets.Any(t => t.Seat == seat))
                        {
                            return seat;
                        }
                    }
                }
            }

            return "";
        }

        [HttpPost]
        public IActionResult UpdateShoppingCart(int cartnr, int newAmount)
        {
            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            var cartItem = shoppingCart.Cart.FirstOrDefault(c => c.Cartnr == cartnr);

            if (cartItem != null)
            {
                cartItem.Amount = newAmount;

                shoppingCart.TotalPrice = shoppingCart.Cart.Sum(c => c.Amount * c.SearchResultVM.Price);

                HttpContext.Session.SetObject("ShoppingCart", shoppingCart);
            }

            return RedirectToAction("Index");
        }
    }
}
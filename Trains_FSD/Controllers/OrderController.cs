using AutoMapper;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Security.Claims;
using Trains_FSD.Areas.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.Util.Mail;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class OrderController : Controller
    {

        private readonly IMapper _mapper;
        private IServiceOrder<Order> _orderService;
        private IEmailSend _sender;
        private IService<TrajectLine> _trajectLineService;
        private readonly SignInManager<User> _signInManager;

        public OrderController(IMapper mapper, IServiceOrder<Order> orderService, 
           IService<TrajectLine> trajectLineService, SignInManager<User> signInManager, IEmailSend sender)
        {
            _mapper = mapper;
            _orderService = orderService;
            _signInManager = signInManager;
            _trajectLineService = trajectLineService;
            _sender = sender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> GetPersonalOrders()
        {

            var userId = _signInManager.UserManager.GetUserId(User);
            var orderList = await _orderService.FindByCustomerId(userId);
            List<OrderVM> orderListVM = _mapper.Map<List<OrderVM>>(orderList);
            orderListVM = orderListVM.OrderByDescending(ol => ol.OrderDate).ToList();

            foreach (OrderVM orderVM in orderListVM)
            {
                foreach (var ticket in orderVM.Tickets)
                {
                    foreach (var details in ticket.TicketDetails)
                    {
                        if ((details.DepartureDate - DateTime.Now).TotalDays < 3)
                        {
                            orderVM.Cancellable = false;
                            break;
                        }
                    }

                }
            }

            return View(orderListVM);

        }

        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = await _orderService.FindById(id);

                    var user = _signInManager.UserManager.GetUserId(User);

                    sendEmail(user, _mapper.Map<OrderVM>(order));
                    order.Confirmed = true;
                    await _orderService.Update(order);
                    return RedirectToAction("GetPersonalOrders");
                }
            }
            catch (DataException ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError(ex.Message, "call system administrator.");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return View();
        }

        public async void sendEmail(string user, OrderVM orderVM)
        {
            var userEmail = (await _signInManager.UserManager.FindByIdAsync(user)).Email;

            string message = @"
            <h1>Thanks for confirming your order ";
            message += User.Identity.Name + @"!</h1> 
            <h3>Here is a summary of your order</h3>
            <ul>
                <li>Order ID: " + orderVM.Id + @"</li>
                <li>Ordered On: " + orderVM.OrderDate.ToString("dd/MM/yyyy") + @"</li>
                <li>Tickets:</li>
                <ul>";

            foreach (var ticket in orderVM.Tickets)
            {
                int count = 1;
                message += @"
                <li>Ticket ID: " + ticket.Id + @"</li>
                <ul>";

                foreach (var ticketDetail in ticket.TicketDetails)
                {
                    var trajectLine = await _trajectLineService.FindById(ticketDetail.TrajectId);
                    TrajectLineVM trajectLineVM = _mapper.Map<TrajectLineVM>(trajectLine);
                    TimeSpan timeDifference = trajectLineVM.Line.ArrivalTime - trajectLineVM.Line.DepartureTime;

                    message += @"Ticket " + count + "<br>" +
                     "<li>Class: " + ticketDetail.Type + "</li>" +
                     "<li>Seat: " + ticketDetail.Seat + "</li>" +
                    "<li>From: " + trajectLineVM.Line.DepartureCity.Name + "</li>" +
                    "<li>To: " + trajectLineVM.Line.ArrivalCity.Name + "</li>" +
                    "<li>Train: " + trajectLineVM.Line.Train.Id + "</li>" +
                    "<li>Departure time: " + ticketDetail.DepartureDate + "</li>" +
                    "<li>Arrival time: " + (ticketDetail.DepartureDate + timeDifference) + "</li>";

                    count++;

                }

                message += @"</ul>";
            }

            message += @"
                            </ul>
                            </ul>
                         
                    </body>
                </html>";

            await _sender.SendEmailAsync(userEmail, "Train E-mail payment complete", message);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _orderService.FindById(id);

            await _orderService.Delete(order);

            return RedirectToAction("GetPersonalOrders");
        }


        //DOORDAT DIT NIET WERK BIJ HET PUBLISHEN VERSTUREN WE DE GEGEVENS VIA EEN MAIL
        //WE VERMOEDEN DAT HET NIET WERK ALS HET OP AZURE STAAT DOORDAT HET EEN 32 BIT SERVER IS WAAROP HET DRAAIT

        //        public async Task<IActionResult> GeneratePDF(int id)
        //        {
        //            var order = await _orderService.FindById(id);
        //            OrderVM orderVM = _mapper.Map<OrderVM>(order);

        //            var html = $@"
        //    <!DOCTYPE html>
        //    <html lang='en'>
        //    <head>
        //        <meta charset='UTF-8'>
        //        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        //        <title>Order Confirmation</title>
        //        <style>
        //            body {{
        //                font-family: Arial, Helvetica, sans-serif;
        //            }}
        //            h1 {{
        //                text-align: center;
        //                color: #008CBA;
        //            }}
        //            ul {{
        //                list-style-type: none;
        //                margin: 0;
        //                padding: 0;
        //            }}
        //            li {{
        //                margin-bottom: 10px;
        //            }}
        //            .ticket {{
        //                border: 1px solid #ddd;
        //                padding: 10px;
        //                margin-bottom: 20px;
        //            }}
        //            .ticket-details {{
        //                margin-left: 20px;
        //                margin-bottom: 20px;
        //                margin-top: 20px;
        //            }}
        //            .ticket-details li {{
        //                margin-bottom: 5px;
        //            }}
        //            .departure-city {{
        //                color: #555;
        //            }}
        //            .train {{
        //                color: #555;
        //            }}
        //        </style>
        //    </head>
        //    <body>
        //        <h1>Thanks for confirming your order!</h1>
        //        <h3>Here is a summary of your order</h3>
        //        <ul>
        //            <li>Order ID: {orderVM.Id}</li>
        //            <li>Ordered On: {orderVM.OrderDate.ToString("dd/MM/yyyy")}</li>
        //            <li>Tickets:</li>
        //            <ul>";

        //            foreach (var ticket in orderVM.Tickets)
        //            {
        //                html += $@"
        //                <li class='ticket'>Ticket ID: {ticket.Id}";
        //                foreach (var ticketDetail in ticket.TicketDetails)
        //                {
        //                    var trajectLine = await _trajectLineService.FindById(ticketDetail.TrajectId);
        //                    TrajectLineVM trajectLineVM = _mapper.Map<TrajectLineVM>(trajectLine);
        //                    TimeSpan timeDifference = trajectLineVM.Line.ArrivalTime - trajectLineVM.Line.DepartureTime;
        //                    html += $@"
        //                        <ul class='ticket-details'>
        //                            <li>Class: {ticketDetail.Type}</li>
        //                            <li>Seat: {ticketDetail.Seat}</li>
        //                            <li>From: {trajectLineVM.Line.DepartureCity.Name}</li>
        //                            <li>To: {trajectLineVM.Line.ArrivalCity.Name}</li>
        //                            <li>Train: {trajectLineVM.Line.Train.Id}</li>
        //                            <li>Departure time: {ticketDetail.DepartureDate}</li>
        //                            <li>Arrival time: {ticketDetail.DepartureDate + timeDifference}</li>
        //                        </ul>
        //                    ";
        //                }
        //                html += "</li>";
        //            }
        //            html += @"
        //            </ul>
        //        </ul>
        //    </body>
        //    </html>
        //";

        //            GlobalSettings globalSettings = new GlobalSettings();
        //            globalSettings.ColorMode = ColorMode.Color;
        //            globalSettings.Orientation = Orientation.Portrait;
        //            globalSettings.PaperSize = PaperKind.A4;
        //            globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 25 };
        //            ObjectSettings objectSettings = new ObjectSettings();
        //            objectSettings.PagesCount = true;
        //            objectSettings.HtmlContent = html;
        //            WebSettings webSettings = new WebSettings();
        //            webSettings.DefaultEncoding = "utf-8";
        //            HeaderSettings headerSettings = new HeaderSettings();
        //            headerSettings.FontSize = 15;
        //            headerSettings.FontName = "Ariel";
        //            FooterSettings footerSettings = new FooterSettings();
        //            footerSettings.FontSize = 12;
        //            footerSettings.FontName = "Ariel";
        //            footerSettings.Center = "Mavi Trains";
        //            objectSettings.HeaderSettings = headerSettings;
        //            objectSettings.FooterSettings = footerSettings;
        //            objectSettings.WebSettings = webSettings;
        //            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
        //            {
        //                GlobalSettings = globalSettings,
        //                Objects = { objectSettings },
        //            };

        //            var pdfFile = _converter.Convert(htmlToPdfDocument);
        //            return File(pdfFile, "application/octet-stream", $"Tickets{orderVM.OrderDate.ToString("dd-MM-yy")}.pdf");
        //        }

    }
}

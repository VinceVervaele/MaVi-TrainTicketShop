using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class HotelController : Controller
    {
        private IConfiguration _Configure;
        private string apiBaseUrl;

        public HotelController(IConfiguration configuration)
        {
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        public async Task<IActionResult> GetHotels(string cityName)
        {
            var hotelVM = new HotelVM();
            apiBaseUrl = apiBaseUrl.Replace("{city}", cityName);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

                using (var response = await httpClient.GetAsync(apiBaseUrl))
                {

                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var hotels = JsonConvert.DeserializeObject<List<HotelInfo>>(apiResponse);

                    hotelVM.HotelResults = hotels != null ? hotels.Select(x => new HotelInfo
                    {
                        display_name = x.display_name?.Substring(0, x.display_name.IndexOf(",")).Trim(),
                        Country = x.display_name?.Substring(x.display_name.LastIndexOf(',') + 1).Trim(),
                        City = cityName
                    }).ToList() : null;
                }
            }

            return View(hotelVM.HotelResults);
        }

        public IActionResult BookHotel(string hotelName)
        {
            TempData["HotelName"] = hotelName;
            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}

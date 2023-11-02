using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Trains_FSD.ViewModels
{
    public class TicketVM
    {
        [Required(ErrorMessage = "Please select a valid departure city.")]
        [DisplayName("Departure City")]
        public int DepartureCityId { get; set; }

        public IEnumerable<SelectListItem>? DepartureCity { get; set; }

        [Required(ErrorMessage = "Please select a valid arrival city.")]
        [DisplayName("Arrival City")]
        public int ArrivalCityId { get; set; }

        public IEnumerable<SelectListItem>? ArrivalCity { get; set; }

        [Required(ErrorMessage = "Please select a valid date.")]
        [DisplayName("Date")]
        public string? DepartureDate { get; set; }

        public string? Class { get; set; }

        public List<SearchResultVM>? SearchResults { get; set; }


    }

}

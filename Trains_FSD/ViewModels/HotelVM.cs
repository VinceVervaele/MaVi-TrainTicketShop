using System.ComponentModel;

namespace Trains_FSD.ViewModels
{
    public class HotelVM
    {
        public List<HotelInfo>? HotelResults { get; set; }
    }

    public class HotelInfo
    {
        [DisplayName("Hotel Name")]
        public string? display_name { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}

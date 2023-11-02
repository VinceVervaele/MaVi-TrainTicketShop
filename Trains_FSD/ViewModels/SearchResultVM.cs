using Trains_FSD.Models.Entities;

namespace Trains_FSD.ViewModels
{
    public class SearchResultVM
    {
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public string? DepartureDate { get; set; }
        public double Price { get; set; }
        public string? Class { get; set; }
        public bool isValid { get; set; } = true;

        public List<LineVM>? Lines { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SearchResultVM other))
            {
                return false;
            }

            return DepartureCity == other.DepartureCity
                && ArrivalCity == other.ArrivalCity
                && DepartureDate == other.DepartureDate;
        }

        public override int GetHashCode()
        {
            return (DepartureCity?.GetHashCode() ?? 0) ^ (ArrivalCity?.GetHashCode() ?? 0) ^ (DepartureDate?.GetHashCode() ?? 0);
        }
    }
}

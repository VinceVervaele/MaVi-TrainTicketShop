using Trains_FSD.Models.Entities;

namespace Trains_FSD.ViewModels
{
    public class LineVM
    {
        public int Id { get; set; }
        public double PriceBusinessClass { get; set; }
        public double PriceEconomyClass { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        
        public int TrajectId { get; set; }

        public TrainVM Train { get; set; }
        public CityVM ArrivalCity { get; set; } 
        public CityVM DepartureCity { get; set; }
    }
}

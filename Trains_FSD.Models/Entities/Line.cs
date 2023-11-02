using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class Line
    {
        public int Id { get; set; }
        public double PriceBusinessClass { get; set; }
        public double PriceEconomyClass { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int TrainId { get; set; }
        public int DepartureCityId { get; set; }
        public int ArrivalCityId { get; set; }

        public virtual City ArrivalCity { get; set; } = null!;
        public virtual City DepartureCity { get; set; } = null!;
        public virtual Train Train { get; set; } = null!;
    }
}

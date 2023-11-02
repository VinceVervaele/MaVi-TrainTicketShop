using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class City
    {
        public City()
        {
            LineArrivalCities = new HashSet<Line>();
            LineDepartureCities = new HashSet<Line>();
            TrajectArrivalCities = new HashSet<Traject>();
            TrajectDepartureCities = new HashSet<Traject>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Line> LineArrivalCities { get; set; }
        public virtual ICollection<Line> LineDepartureCities { get; set; }
        public virtual ICollection<Traject> TrajectArrivalCities { get; set; }
        public virtual ICollection<Traject> TrajectDepartureCities { get; set; }
    }
}

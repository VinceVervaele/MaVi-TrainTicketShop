using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class Traject
    {
        public Traject()
        {
            TicketDetails = new HashSet<TicketDetail>();
            TrajectLines = new HashSet<TrajectLine>();
        }

        public int Id { get; set; }
        public int DepartureCityId { get; set; }
        public int ArrivalCityId { get; set; }

        public virtual City ArrivalCity { get; set; } = null!;
        public virtual City DepartureCity { get; set; } = null!;
        public virtual ICollection<TicketDetail> TicketDetails { get; set; }
        public virtual ICollection<TrajectLine> TrajectLines { get; set; }
    }
}

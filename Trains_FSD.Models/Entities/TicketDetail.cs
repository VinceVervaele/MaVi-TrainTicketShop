using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class TicketDetail
    {
        public int Id { get; set; }
        public string Seat { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int TrajectId { get; set; }
        public int TicketId { get; set; }
        public DateTime DepartureDate { get; set; }

        public virtual Ticket Ticket { get; set; } = null!;
        public virtual Traject Traject { get; set; } = null!;
    }
}

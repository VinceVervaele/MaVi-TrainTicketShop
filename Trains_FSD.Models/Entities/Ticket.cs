using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class Ticket
    {
        public Ticket()
        {
            TicketDetails = new HashSet<TicketDetail>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ICollection<TicketDetail> TicketDetails { get; set; }
    }
}

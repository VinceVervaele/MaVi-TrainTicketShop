using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class Order
    {
        public Order()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string CustomerId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public bool Confirmed { get; set; }

        public virtual AspNetUser Customer { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}

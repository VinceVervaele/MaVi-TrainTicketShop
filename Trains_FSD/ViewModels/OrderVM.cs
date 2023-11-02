using System.ComponentModel;
using Trains_FSD.Services;

namespace Trains_FSD.ViewModels
{
    public class OrderVM
    {
        [DisplayName("Order Id")]
        public int Id { get; set; }
        [DisplayName("Customer")]
        public string CustomerId { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Confirmation Status")]
        public bool Confirmed { get; set; }
        public bool Cancellable { get; set; } = true;

        public ICollection<TicketAddVM> Tickets { get; set; }
    }
}

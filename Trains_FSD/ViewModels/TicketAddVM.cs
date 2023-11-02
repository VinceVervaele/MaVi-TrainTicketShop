namespace Trains_FSD.ViewModels
{
    public class TicketAddVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TrajectId { get; set; }
        public DateTime DepartureDate { get; set; }

        public ICollection<TicketDetailVM> TicketDetails { get; set; }
    }
}

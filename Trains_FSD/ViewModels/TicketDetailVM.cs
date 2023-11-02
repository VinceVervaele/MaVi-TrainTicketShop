namespace Trains_FSD.ViewModels
{
    public class TicketDetailVM
    {
        public string Seat { get; set; }
        public string Type { get; set; }
        public int TrajectId { get; set; }
        public int TicketId { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}

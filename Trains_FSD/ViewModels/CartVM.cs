namespace Trains_FSD.ViewModels
{
    public class CartVM
    {
        public int Cartnr { get; set; }
        public int Amount  { get; set; }
        public double CartPrice { get; set; }
        public SearchResultVM SearchResultVM { get; set; }
        public bool Available { get; set; } = true;
    }
}

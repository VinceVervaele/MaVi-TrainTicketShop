using Trains_FSD.ViewModels;

namespace Trains_FSD.Models
{
    public class LineComparator : IComparer<LineVM>
    {
        public int Compare(LineVM x, LineVM y)
        {
            if (x.ArrivalCity.Name == y.DepartureCity.Name)
            {
                return -1; // x comes before y
            }
            else if (x.DepartureCity.Name == y.ArrivalCity.Name)
            {
                return 1; // x comes after y
            }
            else
            {
                return 0; // x and y are unrelated
            }
        }
    }

}

using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class Train
    {
        public Train()
        {
            Lines = new HashSet<Line>();
        }

        public int Id { get; set; }
        public int CapacityBusinessClass { get; set; }
        public int CapacityEconomyClass { get; set; }

        public virtual ICollection<Line> Lines { get; set; }
    }
}

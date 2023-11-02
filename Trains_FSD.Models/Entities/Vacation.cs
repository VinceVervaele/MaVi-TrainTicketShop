using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class Vacation
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double ModifierPercentage { get; set; }
    }
}

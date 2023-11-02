using System;
using System.Collections.Generic;

namespace Trains_FSD.Models.Entities
{
    public partial class TrajectLine
    {
        public int TrajectId { get; set; }
        public int LineId { get; set; }

        public virtual Traject Traject { get; set; } = null!;
        public virtual Line Line { get; set; } = null!;
    }
}

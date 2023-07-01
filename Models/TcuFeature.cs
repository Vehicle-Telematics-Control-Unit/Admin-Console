using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class TcuFeature
    {
        public long TcuId { get; set; }
        public long FeatureId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Feature Feature { get; set; } = null!;
        public virtual Tcu Tcu { get; set; } = null!;
    }
}

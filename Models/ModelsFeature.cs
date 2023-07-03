using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class ModelsFeature
    {
        public long ModelId { get; set; }
        public long FeatureId { get; set; }
        public bool IsActive { get; set; }

        public virtual Feature Feature { get; set; } = null!;
        public virtual Model Model { get; set; } = null!;
    }
}

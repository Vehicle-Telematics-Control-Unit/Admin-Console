using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class Model
    {
        public Model()
        {
            ModelsFeatures = new HashSet<ModelsFeature>();
            Tcus = new HashSet<Tcu>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ModelsFeature> ModelsFeatures { get; set; }
        public virtual ICollection<Tcu> Tcus { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class Model
    {
        public Model()
        {
            Tcus = new HashSet<Tcu>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Tcu> Tcus { get; set; }
    }
}

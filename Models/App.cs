using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class App
    {
        public App()
        {
            Features = new HashSet<Feature>();
        }

        public string Repo { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public DateTime LatestUpdate { get; set; }
        public string HexDigest { get; set; } = null!;
        public string[]? Volumes { get; set; }
        public string[]? EnvVariables { get; set; }
        public long AppId { get; set; }
        public long? FeatureId { get; set; }
        public string[]? ExposedPorts { get; set; }

        public virtual Feature? Feature { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
    }
}

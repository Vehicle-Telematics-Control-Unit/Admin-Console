using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class Feature
    {
        public Feature()
        {
            Apps = new HashSet<App>();
            TcuFeatures = new HashSet<TcuFeature>();
        }

        public long FeatureId { get; set; }
        public string FeatureName { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; } = null!;
        public long AppId { get; set; }

        public virtual App App { get; set; } = null!;
        public virtual ICollection<App> Apps { get; set; }
        public virtual ICollection<TcuFeature> TcuFeatures { get; set; }
    }
}

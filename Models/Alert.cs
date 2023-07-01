using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class Alert
    {
        public long TcuId { get; set; }
        public string ObdCode { get; set; } = null!;
        public DateTime LogTimeStamp { get; set; }
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual Tcu Tcu { get; set; } = null!;
    }
}

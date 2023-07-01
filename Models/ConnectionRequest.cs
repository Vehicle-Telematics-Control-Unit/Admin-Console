using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class ConnectionRequest
    {
        public long TcuId { get; set; }
        public string DeviceId { get; set; } = null!;
        public DateTime CreationTimeStamp { get; set; }
        public DateTime? VerificationTimeStamp { get; set; }
        public string Token { get; set; } = null!;
        public long StatusId { get; set; }

        public virtual Device Device { get; set; } = null!;
        public virtual RequestStatus Status { get; set; } = null!;
        public virtual Tcu Tcu { get; set; } = null!;
    }
}

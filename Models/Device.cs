using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class Device
    {
        public Device()
        {
            ConnectionRequests = new HashSet<ConnectionRequest>();
            DevicesTcus = new HashSet<DevicesTcu>();
            LockRequests = new HashSet<LockRequest>();
        }

        public string DeviceId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime? LastLoginTime { get; set; }
        public string? IpAddress { get; set; }
        public string? NotificationToken { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
        public virtual ICollection<ConnectionRequest> ConnectionRequests { get; set; }
        public virtual ICollection<DevicesTcu> DevicesTcus { get; set; }
        public virtual ICollection<LockRequest> LockRequests { get; set; }
    }
}

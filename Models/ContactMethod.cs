using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class ContactMethod
    {
        public int Type { get; set; }
        public string Value { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public string UserId { get; set; } = null!;

        public virtual AspNetUser User { get; set; } = null!;
    }
}

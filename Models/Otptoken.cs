using System;
using System.Collections.Generic;

namespace Admin_Console.Models
{
    public partial class Otptoken
    {
        public int Id { get; set; }
        public string Userid { get; set; } = null!;
        public int Token { get; set; }
        public DateTime? Verifiedat { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
    }
}

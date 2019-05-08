using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class Admins
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Privilege { get; set; }
        public string PhotoUrl { get; set; }
    }
}

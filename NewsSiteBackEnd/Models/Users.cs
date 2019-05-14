using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class Users
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string Salt { get; set; }

        public ICollection<Comments> Comments { get; set; }
    }
}

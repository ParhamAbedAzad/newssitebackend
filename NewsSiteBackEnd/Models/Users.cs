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
		public Users(UsersDto u)
		{
			this.Id = u.Id;
			this.Username = u.Username;
			this.Description = u.Description;
			this.PhotoUrl = u.PhotoUrl;
			this.FirstName = u.FirstName;
			this.LastName = u.LastName;
			this.TelNumber = u.TelNumber;
			this.Email = u.Email;
		}

        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public byte[] Salt { get; set; }

        public ICollection<Comments> Comments { get; set; }
    }
}

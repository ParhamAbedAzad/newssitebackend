using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class Admins
    {
        public Admins()
        {
            News = new HashSet<News>();
        }

		public Admins(AdminsDto a)
		{
			Id = a.Id;
			Username = a.Username;
			Email = a.Email;
			Privilege = a.Privilege;
			PhotoUrl = a.PhotoUrl;
		}

		public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Email { get; set; }
        public string Privilege { get; set; }
        public string PhotoUrl { get; set; }
        public byte[] Salt { get; set; }

        public ICollection<News> News { get; set; }
    }
}

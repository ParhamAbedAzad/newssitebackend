using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSiteBackEnd.Models
{
	public class AdminsDto
	{
		public AdminsDto(Admins a)
		{
			Id = a.Id;
			Username = a.Username;
			Email = a.Email;
			Privilege = a.Privilege;
			PhotoUrl = a.PhotoUrl;
		}

		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Privilege { get; set; }
		public string PhotoUrl { get; set; }
		public string Token { get; set; }
	}
}

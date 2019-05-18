using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSiteBackEnd.Models
{
	public class AdminsDto
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Privilege { get; set; }
		public string PhotoUrl { get; set; }
		public string Token { get; set; }
	}
}

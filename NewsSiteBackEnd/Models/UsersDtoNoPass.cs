using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSiteBackEnd.Models
{
	public class UsersDtoNoPass
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string TelNumber { get; set; }
		public string Email { get; set; }
		public string token { get; set; }
	}
}

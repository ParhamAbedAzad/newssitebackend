﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSiteBackEnd.Models
{
	public class UsersDto
	{

		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[Phone(ErrorMessage = "not a valid PNUMBER")]
		public string TelNumber { get; set; }
		[EmailAddress(ErrorMessage = "not a valid email address")]
		public string Email { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public string Token { get; set; }

	}
}

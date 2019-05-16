using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
	public class CommentsDto
	{
		public int Id { get; set; }
		public string Body { get; set; }
		public DateTime? Date { get; set; }
		public int? UserId { get; set; }
		public int? NewsId { get; set; }

	}
}

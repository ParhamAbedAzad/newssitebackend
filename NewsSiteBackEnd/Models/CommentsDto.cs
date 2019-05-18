using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
	public class CommentsDto
	{
		public CommentsDto(Comments c)
		{
			Id = c.Id;
			Body = c.Body;
			Date = c.Date;
			UserId = c.UserId;
			NewsId = c.NewsId;
		}

		public int Id { get; set; }
		public string Body { get; set; }
		public DateTime? Date { get; set; }
		public int? UserId { get; set; }
		public int? NewsId { get; set; }

	}
}

using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
	public class TagsDto
	{
		public int Id { get; set; }
		public int? NewsId { get; set; }
		public string Tag { get; set; }
		
	}
}

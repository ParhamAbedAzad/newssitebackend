using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
	public class TagsDto
	{
		public TagsDto(Tags tag)
		{
			Id = tag.Id;
			NewsId = tag.NewsId;
			Tag = tag.Tag;
		}
		public int Id { get; set; }
		public int? NewsId { get; set; }
		public string Tag { get; set; }
		
	}
}

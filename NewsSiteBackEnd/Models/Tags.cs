using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class Tags
    {
		public Tags(TagsDto t)
		{
			Id = t.Id;
			NewsId = t.NewsId;
			Tag = t.Tag;
		}

		public int Id { get; set; }
        public int? NewsId { get; set; }
        public string Tag { get; set; }

        public News News { get; set; }
    }
}

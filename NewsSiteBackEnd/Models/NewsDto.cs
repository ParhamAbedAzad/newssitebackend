using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSiteBackEnd.Models
{
	public class NewsDto
	{
		public NewsDto()
		{

		}
		public NewsDto(News news)
		{
			Id = news.Id;
			Title = news.Title;
			Text = news.Text;
			DateAdded = news.DateAdded;
			AdminId = news.AdminId;
		}
		public int Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public DateTime? DateAdded { get; set; }
		public int? AdminId { get; set; }

		
	}
}

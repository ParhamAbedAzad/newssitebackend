using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSiteBackEnd.Models;

namespace NewsSiteBackEnd.Controllers
{
	[Route("News")]
    public class NewsController : Controller
    {
		private NEWS_SITEContext dbContext;
		public NewsController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public IActionResult getAll()
		{
			return Ok(dbContext.News);
		}
		[HttpGet("{start}/{end}")]
		public IActionResult getRange([FromRoute(Name = "start")]int start, [FromRoute(Name = "end")]int end )
		{
			start--;
			var res = dbContext.News.OrderByDescending(n => n.DateAdded).Skip(start).Take(end);
			if (!res.Any())
				return BadRequest();
			return Ok(res);
		}
		[HttpGet("{id}")]
        public IActionResult getNews([FromRoute(Name = "id")]int newsId)
        {
			var news = dbContext.News.Find(newsId);
			
			if (news == null)
				return NotFound("News not found");
			return Ok(news);
        }
		[Authorize(Roles ="admin")]
		[HttpPost]
		public IActionResult addNews([FromBody]News news)
		{
			news.DateAdded = DateTime.Now;
			dbContext.News.Add(news);
			dbContext.SaveChanges();
			return Ok();
		}
		[Authorize(Roles = "admin")]
		[HttpGet("del/{id}")]
		public IActionResult delNews([FromRoute(Name = "id")]int newsId)
		{
			var news = dbContext.News.Find(newsId);
			if (news == null)
			{
				return NotFound("News not found");
			}
			dbContext.News.Remove(news);
			dbContext.Comments.RemoveRange(dbContext.Comments.Where(c => c.NewsId == newsId)); // remove comments of a news upon delete too
			dbContext.SaveChanges();
			return Ok();

		}
		[HttpGet("newsComments/{newsid}")]
		public IActionResult getNewsComments([FromRoute(Name ="newsid")]int newsid)
		{

			News news = dbContext.News.Find(newsid);
			if (news == null)
			{
				return BadRequest("news not found");
			}
			return Ok(news.Comments);
		}

	}
}
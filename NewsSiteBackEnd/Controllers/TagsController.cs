using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSiteBackEnd.Models;

namespace NewsSiteBackEnd.Controllers
{
	[Route("Tags")]
	public class TagsController : Controller
	{
		private NEWS_SITEContext dbContext;
		public TagsController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[AllowAnonymous]
		[HttpGet("newsTags/{newsid}")]
		public IActionResult getNewsTags([FromRoute(Name = "newsid")]int newsid )
        {
		//WIP	
			if (dbContext.News.Find(newsid) != null)
			{
				var tags = dbContext.Tags.Where(t => t.NewsId == newsid);
				var query = from t in tags
							select new { t.Id , t.Tag , t.NewsId};
				return Ok(query);
				
			}
			return BadRequest("no news with such id");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
				var query = from n in dbContext.News.OrderByDescending(n => n.DateAdded)
							select new NewsDto(n);
			return Ok(query);
		}
		[HttpGet("{start}/{end}")]
		public IActionResult getRange([FromRoute(Name = "start")]int start, [FromRoute(Name = "end")]int end )
		{
			start--;
			var res = dbContext.News.OrderByDescending(n => n.DateAdded).Skip(start).Take(end);
			if (!res.Any())
				return BadRequest();
			var query = from n in res select new NewsDto(n);
			return Ok(query);
		}
		[HttpGet("page/{num}")]
		public IActionResult getByPage([FromRoute(Name = "num")]int num)
		{
			num--;
			var res = dbContext.News.OrderByDescending(n => n.DateAdded).Skip(num*6).Take(6);
			if (!res.Any())
				return BadRequest();
			var query = from n in res select new NewsDto(n);
			return Ok(query);
		}
		[HttpGet("page")]
		public IActionResult totalPages()
		{
			var count = dbContext.News.Count();
			
			return Ok(count);
		}
		[HttpGet("{id}")]
        public IActionResult getNews([FromRoute(Name = "id")]int newsId)
        {
			var news = dbContext.News.Find(newsId);
			
			if (news == null)
				return NotFound("News not found");
			return Ok(new NewsDto(news));
        }
		[Authorize(Roles = "admin,adminFullAccess")]
		[HttpPost]
		public IActionResult addNews([FromBody]News news)
		{
			if (news == null || string.IsNullOrEmpty(news.Text)){
				return BadRequest("news or  body can not be empty");
			}
			int adminid = Int32.Parse(this.User.FindFirst("adminid").Value);
			news.AdminId = adminid;
			if (dbContext.Admins.Find(adminid) == null)
			{
				return BadRequest("could not resolve admin");
			}

			news.DateAdded = DateTime.Now;
			dbContext.News.Add(news);
			dbContext.SaveChanges();
			return Ok();
		}

		[Authorize(Roles = "admin,adminFullAccess")]
		[HttpDelete("{id}")]
		public IActionResult delNews([FromRoute(Name = "id")]int newsId)
		{
			var news = dbContext.News.Find(newsId);
			if (news == null)
			{
				return NotFound("News not found");
			}
			dbContext.NewsPhoto.RemoveRange(dbContext.NewsPhoto.Where(n => n.NewsId == newsId)); //removes related pics
			dbContext.Comments.RemoveRange(dbContext.Comments.Where(c => c.NewsId == newsId)); // remove comments of a news upon delete too
			dbContext.Tags.RemoveRange(dbContext.Tags.Where(t => t.NewsId == newsId));
			dbContext.News.Remove(news);
			dbContext.SaveChanges();
			return Ok();

		}

		[AllowAnonymous]
		[HttpGet("adminNews/{adminid}")]
		public IActionResult getAdminNews([FromRoute(Name = "adminid")]int adminId)
		{
			Admins admin = dbContext.Admins.Find(adminId);
			if (admin == null)
			{
				return BadRequest("admin not found");
			}
			var news = dbContext.News.Where(n => n.AdminId == admin.Id);
			var query = from n in news
						select new { id = n.Id, title = n.Title, text = n.Text, adminid = n.AdminId, tags = n.Tags };
			return Ok(query);
		}

		[HttpGet("adminNews/u/{adminuname}")]
		public IActionResult getAdminNews([FromRoute(Name = "adminuname")]string adminUserName)
		{
			Admins admin = dbContext.Admins.Single(a => a.Username == adminUserName);
			if (admin == null)
			{
				return BadRequest("admin not found");
			}
			
			var news = dbContext.News.Where(n => n.AdminId == admin.Id);
			var query = from n in news
						select new { id=n.Id ,  title = n.Title, text = n.Text , adminid = n.AdminId , tags = n.Tags };
			return Ok(query);
		}

	}
}
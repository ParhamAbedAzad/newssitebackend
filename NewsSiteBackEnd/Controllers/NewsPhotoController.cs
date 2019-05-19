using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSiteBackEnd.Models;

namespace NewsSiteBackEnd.Controllers
{
	[Route("NewsPhoto")]
    public class NewsPhotoController : Controller
    {
		private NEWS_SITEContext dbContext;
		public NewsPhotoController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}
		
		[HttpGet]
		public IActionResult getAllPics()
        {
			var pths = dbContext.NewsPhoto;
			/*
			var query = from c in comments
						select new { c.Id, c.Body, c.Date, c.UserId };*/
			return Ok(pths);

		}
		[HttpGet("n/{newsid}")]
		public IActionResult getByNewsId([FromRoute(Name ="newsid")]int newsid)
		{
			if (dbContext.News.Find(newsid) == null)
				return BadRequest("news not found");
			var pths = dbContext.NewsPhoto.Where(n => n.NewsId == newsid);
			
			var query = from p in pths
						select new {p.Id , p.PhotoUrl , p.NewsId };
			return Ok(query);
		}
		[Authorize(Roles = "admin,adminFullAccess")]
		[HttpPost]
		public IActionResult addPhoto([FromBody]NewsPhoto newsPhoto)
		{
			if(dbContext.News.Find(newsPhoto.NewsId) != null || !string.IsNullOrEmpty(newsPhoto.PhotoUrl))
			{
				dbContext.NewsPhoto.Add(newsPhoto);
				dbContext.SaveChanges();
				return Ok();
			}
			return BadRequest("news not found or photourl is emtpy");
		}
		[Authorize(Roles = "admin,adminFullAccess")]
		[HttpDelete("{photoid}")]
		public IActionResult delete([FromRoute(Name = "photoid")]int photoid)
		{
			if (dbContext.NewsPhoto.Find(photoid) != null )
			{
				var photo = dbContext.NewsPhoto.Find(photoid);
				dbContext.NewsPhoto.Remove(photo);
				dbContext.SaveChanges();
				return Ok("photo removed");
			}
			return BadRequest("newsphoto not found ");
		}
		[Authorize(Roles = "admin,adminFullAccess")]
		[HttpDelete("url/{photourl}")]
		public IActionResult delete([FromRoute(Name = "photourl")]string photourl)
		{
			NewsPhoto photo = dbContext.NewsPhoto.SingleOrDefault(np => np.PhotoUrl == photourl);
			if (photo != null)
			{
				
				dbContext.NewsPhoto.Remove(photo);
				dbContext.SaveChanges();
				return Ok("photo removed");
			}
			return BadRequest("newsphoto not found ");
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSiteBackEnd.Models;
namespace NewsSiteBackEnd.Controllers
{
	[Authorize]
	[Route("Comments")]
    public class CommentsController : Controller
    {
		private NEWS_SITEContext dbContext;
		public CommentsController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[Authorize(Roles = "user")]
		[HttpPost]
		public IActionResult addComment([FromBody]Comments comment)
		{
			int userId = Int32.Parse(this.User.FindFirst("userid").Value);
			if(string.IsNullOrEmpty(comment.Body))
			{
				return BadRequest("comment has no text");
			}

			comment.UserId = userId;

			if(dbContext.Users.Find(comment.UserId) == null || dbContext.News.Find(comment.NewsId) == null)
			{
				return BadRequest("user or news does not exist");
			}
			comment.Date = DateTime.Now;
			dbContext.Comments.Add(comment);
			dbContext.SaveChanges();
			return Ok();
		}

		[HttpDelete("{commentId}")]		
		public IActionResult delComment([FromRoute(Name = "commentID")]int commentId) {

			var comment = dbContext.Comments.Find(commentId);

			if( comment == null){
				return NotFound("comment not found");
			}
			dbContext.Comments.Remove(comment);
			dbContext.SaveChanges();
			return Ok();
		}
		[HttpPost("Edit")]
		public IActionResult editComment([FromBody]Comments comment)
		{
			if(comment.Id == null || comment.Body == null)
			{
				return BadRequest("Comment not found");
			}
			
			var cmnt = dbContext.Comments.Find(comment.Id);
			if(cmnt == null)
			{
				return NotFound();
			}
			if (comment.UserId != cmnt.UserId)
			{
				return Unauthorized();
			}
			cmnt.Body = comment.Body;
			dbContext.Comments.Update(cmnt);
			dbContext.SaveChanges();
			return Ok();
		}

		[AllowAnonymous]
		[HttpGet("newsComments/{newsid}")]
		public IActionResult getAllComments([FromRoute(Name ="newsid")]int newsid)
		{
			if (dbContext.News.Find(newsid) != null)
			{
				
				var comments = dbContext.Comments.Where(c => c.NewsId == newsid);
				var query = from c in comments
							select new CommentsDto(c);
				return Ok(query);
			}
			return BadRequest("no news with such id");
		}
		[AllowAnonymous]
		[HttpGet("userComments/{userid}")]
		public IActionResult getUserComments([FromRoute(Name = "userid")]int userid)
		{
			Users user = dbContext.Users.Find(userid);
			if (user == null)
			{
				return BadRequest("user not found");
			}
			var comments = dbContext.Comments.Where(c => c.UserId == userid);
			var query = from c in comments
						select new CommentsDto(c);//{ c.Id, c.Body, c.Date, c.UserId };
			return Ok(query);
		}

	}
}
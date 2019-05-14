using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsSiteBackEnd.Models;
namespace NewsSiteBackEnd.Controllers
{
	[Route("Comment")]
    public class CommentsController : Controller
    {
		private NEWS_SITEContext dbContext;
		public CommentsController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpPost]
		public IActionResult addComment([FromBody]Comments comment)
		{
			if(string.IsNullOrEmpty(comment.Body))
			{
				return BadRequest("comment has no text");
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

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsSiteBackEnd.Models;

namespace NewsSiteBackEnd.Controllers
{
	[Route("radio")]
	public class radioController : Controller
    {
		private NEWS_SITEContext dbContext;
		public radioController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpGet]
		public IActionResult playRadio()
		{
			string url = dbContext.RadioUrl.FirstOrDefault(id => id.Id == 1).Url;
			if (url == null)
			{
				return BadRequest();
			}
			string path = "./Resources/" + url + ".mp3";
			if (!System.IO.File.Exists(path) )
			{
				return BadRequest();
			}
			return File(System.IO.File.OpenRead(path), "audio/mp3");
			
		}
		[HttpGet("tracks")]
		public IActionResult getTracks()
		{
			var query = (from rad in dbContext.RadioUrl orderby rad.Id descending select new { url = rad.Url }).Take(6);
			return Ok(query);

		}
		/*
		[HttpPost]
		public IActionResult addFile()
		{


			return Ok();
		}*/
	}
}
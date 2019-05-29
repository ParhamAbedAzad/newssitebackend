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
			return File(System.IO.File.OpenRead("teddy.mp3"), "audio/mp3");
		}
	}
}
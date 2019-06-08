using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using NewsSiteBackEnd.Models;
namespace NewsSiteBackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
			/*using (NEWS_SITEContext n = new NEWS_SITEContext())
			{

				Users user = new Users()
				{
					Username = "amir",
					Email = "amirmohammad.abedini@gmail.com",
					Password = "da6845g8ag4ad4g"
					,Description = "AMIR IS CUTE AF"
				};
				n.Add(user);
				n.SaveChanges();
			}*/
			CreateWebHostBuilder(args).Build().Run();
			
		}

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

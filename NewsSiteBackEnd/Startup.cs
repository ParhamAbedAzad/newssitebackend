using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NewsSiteBackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using NewsSiteBackEnd.Models;
using Newtonsoft.Json;

namespace NewsSiteBackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
		private NEWS_SITEContext dbContext;
		

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
			{

				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = "ourBeautifulNewsSite",
					ValidAudience = "user",
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("lovelye_icecream_pincess_sweetie"))
				};
			});

			/*services.AddAuthorization(options =>
			{
				options.AddPolicy("Admins", policy =>
								  policy.RequireClaim("adminid",dbContext.Admins.All);
			});
			*/

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddDbContext<NEWS_SITEContext>(options => options.UseSqlServer("Server=185.252.30.32;Database=NEWS_SITE;Persist Security Info=True;User ID=izad;Password=Izadizadi1742"));
			/*services.AddMvc().AddJsonOptions(options => {
				//options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});*/
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
			app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
			app.UseAuthentication();
			app.UseHttpsRedirection();
            app.UseMvc();

        }


    }
}

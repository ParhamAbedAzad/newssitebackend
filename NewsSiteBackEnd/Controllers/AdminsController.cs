using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsSiteBackEnd.Models;
namespace NewsSiteBackEnd.Controllers
{
	[Authorize]
	[ApiController]
	[Route("Users")]
	public class AdminsController : Controller
    {
		private NEWS_SITEContext dbContext;
		public AdminsController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[AllowAnonymous]
		[HttpPost("authenticate")]
		public IActionResult Authenticate([FromBody]UsersDto userDto)
		{
			if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password))
			{
				return BadRequest("username or pass is null or empty");
			}
			var user = dbContext.Users.SingleOrDefault(x => x.Username == userDto.Username);
			if (user == null)
				return BadRequest(new { message = "Username or password is incorrect" });

			System.Diagnostics.Debug.WriteLine("SHM:" + user.Password + "xxxxxx" + user.Salt + " ++++");
			if (!VerifyPasswordHash(userDto.Password, user.Password, user.Salt))
			{
				return BadRequest(new { message = "Username or password is incorrect" });
			}


			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("lovelye_icecream_pincess_sweetie");
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = "ourBeautifulNewsSite",
				Audience = "user",
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Role, "user")

				}),
				Expires = DateTime.UtcNow.AddHours(3),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);
			UsersDtoNoPass userTokenHolder = new UsersDtoNoPass();
			var config = new MapperConfiguration(cfg => cfg.CreateMap<Users, UsersDtoNoPass>());
			var mapper = config.CreateMapper();
			mapper.Map(user, userTokenHolder);

			userTokenHolder.token = tokenString;

			return Ok(userTokenHolder);

		}
		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}


		private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
			if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected is " + storedHash.Length + " " + ").", "passwordHash");
			if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected is " + storedSalt.Length + " ).", "passwordHash");

			using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

				for (int i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != storedHash[i]) return false;
				}

			}

			return true;
		}
	}
}
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
	[Authorize(Roles = "admin,adminFullAccess")]
	//[ApiController]
	[Route("Admins")]
	public class AdminsController : Controller
    {
		private NEWS_SITEContext dbContext;
		public AdminsController(NEWS_SITEContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[AllowAnonymous]
		[HttpPost("authenticate")]
		public IActionResult Authenticate([FromBody]AdminsDto adminDto)
		{
			if (string.IsNullOrEmpty(adminDto.Username) || string.IsNullOrEmpty(adminDto.Password))
			{
				return BadRequest("username or pass is null or empty");
			}
			var admin = dbContext.Admins.SingleOrDefault(x => x.Username == adminDto.Username);
			if (admin == null)
				return BadRequest(new { message = "Username or password is incorrect" });

			System.Diagnostics.Debug.WriteLine("SHM:" + admin.Password + "xxxxxx" + admin.Salt + " ++++");
			if (!VerifyPasswordHash(adminDto.Password, admin.Password, admin.Salt))
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
					new Claim(ClaimTypes.Role, "admin"),
					new Claim("adminid",admin.Id.ToString())
				}),
				Expires = DateTime.UtcNow.AddHours(3),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			if(admin.Privilege == "fullAccess")
			{
				tokenDescriptor = new SecurityTokenDescriptor
				{
					Issuer = "ourBeautifulNewsSite",
					Audience = "user",
					Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Role, "adminFullAccess"),
					new Claim("adminid",admin.Id.ToString())
				}),
					Expires = DateTime.UtcNow.AddHours(3),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};
			}

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);
			AdminsDto adminTokenHolder = new AdminsDto();
			var config = new MapperConfiguration(cfg => cfg.CreateMap<Admins, AdminsDto>());
			var mapper = config.CreateMapper();
			mapper.Map(admin, adminTokenHolder);
			adminTokenHolder.Password = null;
			
			adminTokenHolder.Token = tokenString;

			return Ok(adminTokenHolder);

		}
		[Authorize(Roles = "adminFullAccess")]
		[HttpPost("regAdmin")]
		public IActionResult Register([FromBody]AdminsDto adminDto)
		{
			if (dbContext.Admins.Any(x => x.Username == adminDto.Username))
			{
				return BadRequest("username already taken");
			}

			byte[] passwordHash, passwordSalt;
			CreatePasswordHash(adminDto.Password, out passwordHash, out passwordSalt);
			var config = new MapperConfiguration(cfg => cfg.CreateMap<AdminsDto, Admins>());
			var mapper = config.CreateMapper();
			Admins admin = new Admins();
			mapper.Map(adminDto, admin);
			admin.Password = passwordHash;
			admin.Salt = passwordSalt;

			dbContext.Admins.Add(admin);
			dbContext.SaveChanges();
			return Ok("admin: " + admin.Username + " successfully created");

		}
		[Authorize(Roles = "adminFullAccess")]
		[HttpDelete("{id}")]
		public IActionResult deleteAdmin([FromRoute(Name ="id")]int id)
		{
			Admins admin = dbContext.Admins.Find(id);
			if(admin == null)
			{
				return BadRequest("admin not found");
			}
			if(admin.Privilege == "fullAccess")
			{
				return BadRequest("can not delete a full Access admin");
			}
			dbContext.Admins.Remove(admin);
			dbContext.SaveChanges();
			return Ok();
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
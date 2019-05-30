using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSiteBackEnd.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AutoMapper;
using System.Net.Http.Headers;
using System.IO;

namespace NewsSiteBackEnd.Controllers
{
	[Authorize]
	[ApiController]
	[Route("Users")]
    public class UsersController : Controller
    {
		private NEWS_SITEContext dbContext;
		public UsersController(NEWS_SITEContext dbContext)
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
			if(user == null)
				return BadRequest(new { message = "Username or password is incorrect" });

			//System.Diagnostics.Debug.WriteLine("SHM:" +user.Password +"xxxxxx" + user.Salt +" ++++");
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
					new Claim(ClaimTypes.Role, "user"),
					new Claim("userid",user.Id.ToString())
				
				}),
				Expires = DateTime.UtcNow.AddHours(3),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);
			UsersDtoNoPass userTokenHolder = new UsersDtoNoPass(user);

			userTokenHolder.Token = tokenString;

			return Ok(userTokenHolder);

		}

		[AllowAnonymous]
		[HttpPost("register")]
		public IActionResult Register([FromBody]UsersDto userDto)
		{
			if(dbContext.Users.Any(x => x.Username == userDto.Username))
			{
				return BadRequest("username already taken");
			}
			if (dbContext.Users.Any(x => x.Email == userDto.Email))
			{
				return BadRequest("Email Address already taken");
			}
			if (dbContext.Users.Any(x => x.TelNumber == userDto.TelNumber))
			{
				return BadRequest("tell number already taken");
			}

			byte[] passwordHash, passwordSalt;
			CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
			var config = new MapperConfiguration(cfg => cfg.CreateMap<UsersDto, Users>());
			var mapper = config.CreateMapper();
			Users user = new Users();
			mapper.Map(userDto, user);
			user.Password = passwordHash;
			user.Salt = passwordSalt;

			dbContext.Users.Add(user);
			dbContext.SaveChanges();
			return Ok("user: " + user.Username + " successfully created");

		}

		[Authorize(Roles = "admin")]
		[HttpGet("{id}")]
		public IActionResult getUser([FromRoute(Name ="id")]int userId)
        {
			if(Int32.Parse(this.User.FindFirst("userid").Value) == userId)
			{
				return Unauthorized();
			}
			var user = dbContext.Users.Find(userId);
			if(user == null)
			{
				return BadRequest("user not found");
			}

			UsersDtoNoPass u = new UsersDtoNoPass(user);

			return Ok(u);
        }
		[Authorize(Roles = "admin")]
		[HttpGet]
		public IActionResult getAll()
		{
			var users = dbContext.Users;
			return Ok(users);
		}

		[Authorize(Roles ="admin")]
		[HttpDelete("{id}")]
		public IActionResult DeleteUser(int id)
		{
			var user = dbContext.Users.Find(id);
			if(user != null)
			{
				dbContext.Comments.RemoveRange(dbContext.Comments.Where(c => c.UserId == user.Id));
				dbContext.Users.Remove(user);
				dbContext.SaveChanges();
				return Ok();
			}
			return NotFound("user not found");
		}
		[Authorize(Roles = "admin,adminFullAccess")]
		[HttpPut("{id}")]
		public IActionResult UpdateUser(int id, [FromBody]UsersDto userDto) {
			var config = new MapperConfiguration(cfg => cfg.CreateMap<UsersDto, Users>());
			var mapper = config.CreateMapper();
			Users newUser = new Users();
			mapper.Map(userDto, newUser);
			var dbExistingUser = dbContext.Users.Find(id);
			if(dbExistingUser == null)
			{
				return NotFound();
			}
			if(dbExistingUser.Username != newUser.Username)
			{
				if (dbContext.Users.Any(x => x.Username == newUser.Username))
				{
					return BadRequest("this username is taken");
				}
			}
			if (dbExistingUser.Email != newUser.Email)
			{
				if (dbContext.Users.Any(x => x.Email == newUser.Email))
				{
					return BadRequest("this email is taken");
				}
			}
			dbExistingUser.Username = newUser.Username;
			dbExistingUser.Email = newUser.Email;
			dbExistingUser.FirstName = newUser.FirstName;
			dbExistingUser.LastName = newUser.LastName;
			dbExistingUser.TelNumber = newUser.TelNumber;
			dbExistingUser.Description = newUser.Description;
			dbExistingUser.PhotoUrl = newUser.PhotoUrl;
			if (!string.IsNullOrWhiteSpace(userDto.Password))
			{
				byte[] passwordHash, passwordSalt;
				CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

				dbExistingUser.Password = passwordHash;
				dbExistingUser.Salt = passwordSalt;
				dbContext.Users.Update(dbExistingUser);
				dbContext.SaveChanges();
				return Ok();
			}
			return BadRequest("empty pswd");
		}
		
		
		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			if (password == null) throw new ArgumentNullException("password empty");
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
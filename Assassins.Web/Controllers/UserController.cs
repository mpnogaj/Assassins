using Assassins.Web.Dto;
using Assassins.Web.Middlewares;
using Assassins.Web.Models;
using Assassins.Web.Services.JwtService;
using Assassins.Web.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assassins.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IJwtService _jwtService;

		public UserController(IUserService userService, IJwtService jwtService)
		{
			_userService = userService;
			_jwtService = jwtService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var user = await _userService.Login(loginDto);
			if (user == null)
			{
				return Unauthorized();
			}

			return Ok(CreateJwt(user));
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
		{
			var user = await _userService.Register(userRegisterDto);

			if (user == null)
			{
				return Forbid();
			}

			return Ok(CreateJwt(user));
		}

		[HttpGet("isLoggedIn")]
		[Authorize]
		public IActionResult LoggedIn()
		{
			return Ok();
		}

		[HttpPost("logout")]
		[Authorize]
		public IActionResult Logout()
		{
			Response.Cookies.Delete("jwt");
			return Ok();
		}

		[HttpGet("userInfo")]
		[Authorize]
		public IActionResult UserInfo()
		{
			var user = HttpContext.GetLoggedUser();
			if (user == null)
			{
				return Unauthorized();
			}

			var userDto = new UserDto
			{
				Id = user.Id,
				FullName = user.FullName
			};
			return Ok(userDto);
		}

		private string CreateJwt(User user)
		{
			var jwtToken = _jwtService.CreateJwtToken(user);

			Response.Cookies.Append("jwt", jwtToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = false
			});

			return jwtToken;
		}
	}
}

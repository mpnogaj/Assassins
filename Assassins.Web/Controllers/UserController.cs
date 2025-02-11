using System.Reflection.Metadata.Ecma335;
using Assassins.Web.Dto;
using Assassins.Web.Middlewares;
using Assassins.Web.Models;
using Assassins.Web.Services.JwtService;
using Assassins.Web.Services.RecaptchaService;
using Assassins.Web.Services.UserService;
using Assassins.Web.Utils;
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
		private readonly IRecaptchaService _recaptchaService;

		public UserController(IUserService userService, IJwtService jwtService, IRecaptchaService recaptchaService)
		{
			_userService = userService;
			_jwtService = jwtService;
			_recaptchaService = recaptchaService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var loginResult = await _userService.Login(loginDto);

			return loginResult.Match(
				onSuccess: (user) => Ok(CreateJwt(user)),
				onFailure: (error) =>
					error switch
					{
						UserServiceErrors.UsernameNotFoundError or UserServiceErrors.InvalidPasswordError
							=> Unauthorized("Incorrect username or password"),
						_ => Problem("An unknown error occurred", statusCode: StatusCodes.Status500InternalServerError)
					}
			);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
		{
			var recaptchaResult = await _recaptchaService.ValidateRecaptcha(userRegisterDto.RecaptchaToken);

			return await recaptchaResult.MatchAsync(
				onSuccess: async () =>
				{
					var userResult = await _userService.Register(userRegisterDto);

					return userResult.Match(
						(user) => Ok(CreateJwt(user)),
						(error) => error switch
						{
							UserServiceErrors.PasswordTooShortError => BadRequest(
								"Password to short. Please use longer one"),
							UserServiceErrors.UsernameTakenError => Conflict(
								"Username is already taken"),
							_ => Problem("An unknown error occurred",
								statusCode: StatusCodes.Status500InternalServerError)
						}
					);
				},
				onFailure: error => error switch
				{
					RecaptchaServiceErrors.VerificationFailed
						or RecaptchaServiceErrors.VerificationApiReturnedNonSuccessStatusCode
						=> StatusCode(StatusCodes.Status403Forbidden, "reCAPTCHA verification failed"),
					RecaptchaServiceErrors.RecaptchaSecretMissingError
						=> StatusCode(StatusCodes.Status500InternalServerError,
							"reCAPTCHA secret missing! Contact site admin"),
					_ => Problem("An unknown error occurred", statusCode: StatusCodes.Status500InternalServerError)
				});
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
			return HttpContext.GetLoggedUser().Match<IActionResult>(
				onSuccess: (user) => Ok(new UserDto
				{
					Id = user.Id,
					FullName = user.FullName
				}),
				onFailure: (_) => Unauthorized());
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

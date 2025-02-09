using System.Security.Claims;
using Assassins.Web.Models;
using Assassins.Web.Services.Repositories.UserRepository;

namespace Assassins.Web.Middlewares;

public class UserMiddleware
{
	public const string UserKey = "User";

	private readonly RequestDelegate _next;

	public UserMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var username = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

		if (!string.IsNullOrEmpty(username))
		{
			var userRepository = context.RequestServices.GetRequiredService<IUserRepository>();
			var user = await userRepository.GetUser(username);

			if (user != null)
			{
				context.Items[UserKey] = user;
			}
		}

		await _next(context);
	}
}

public static class UserMiddlewareExtensions
{
	public static IApplicationBuilder UseUser(this IApplicationBuilder app)
	{
		app.UseMiddleware<UserMiddleware>();
		return app;
	}

	public static User? GetLoggedUser(this HttpContext httpContext)
	{
		return httpContext.Items[UserMiddleware.UserKey] as User;
	}
}
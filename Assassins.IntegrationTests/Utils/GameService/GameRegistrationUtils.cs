using Assassins.Web.Db;
using Microsoft.EntityFrameworkCore;

namespace Assassins.IntegrationTests.Utils.GameService;

public static class GameRegistrationUtils
{
	public static async Task<bool> IsUserRegistered(IServiceProvider services, string username)
	{
		using var scope = services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);

		return user?.Registered ?? false;
	}
}
using Assassins.IntegrationTests.Consts;
using Assassins.IntegrationTests.Utils.User;
using Assassins.Web.Db;

namespace Assassins.IntegrationTests.Fixtures;
public class WebAppWithRegisteredAndLoggedUsers : CustomWebApplicationFactory, IAsyncLifetime
{
	public List<UserContext> LoggedInUsers { get; }
	public UserContext AdminUser { get; }
	public UserContext UnauthorizedUser { get; }

	public WebAppWithRegisteredAndLoggedUsers()
	{
		LoggedInUsers = new List<UserContext>
		{
			new()
			{
				UserInfo = UserInfos.User1,
				HttpClient = CreateClient()
			},
			new()
			{
				UserInfo = UserInfos.User2,
				HttpClient = CreateClient()
			}
		};

		UnauthorizedUser = new UserContext
		{
			UserInfo = UserInfos.User3,
			HttpClient = CreateClient()
		};

		AdminUser = new UserContext()
		{
			UserInfo = UserInfos.AdminUser,
			HttpClient = CreateClient()
		};
	}

	public async Task InitializeAsync()
	{
		await ResetDatabaseAsync();
		await LoginUser(AdminUser);
		foreach (var user in LoggedInUsers)
		{
			await RegisterUser(user);
		}
	}

	Task IAsyncLifetime.DisposeAsync()
	{
		return Task.CompletedTask;
	}

	private async Task LoginUser(UserContext userContext)
	{
		var httpClient = userContext.HttpClient;
		var registerResponse = await httpClient.PostAsJsonAsync(Routes.User.Login, userContext.UserInfo.ToLoginDto());
		Assert.True(registerResponse.IsSuccessStatusCode);
	}

	private async Task RegisterUser(UserContext userContext)
	{
		var httpClient = userContext.HttpClient;
		var registerResponse = await httpClient.PostAsJsonAsync(Routes.User.Register, userContext.UserInfo.ToUserRegisterDto());
		Assert.True(registerResponse.IsSuccessStatusCode);
	}

	private async Task ResetDatabaseAsync()
	{
		using var scope = Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		await dbContext.Database.EnsureDeletedAsync();
		await dbContext.Database.EnsureCreatedAsync();
	}
}
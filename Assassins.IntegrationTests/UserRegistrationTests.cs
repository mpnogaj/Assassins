using Assassins.IntegrationTests.Consts;
using Assassins.IntegrationTests.Utils.User;

namespace Assassins.IntegrationTests;

public class UserRegistrationTests : IClassFixture<CustomWebApplicationFactory>
{
	private readonly CustomWebApplicationFactory _factory;

	public UserRegistrationTests(CustomWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task IsLoggedInDoesNotSendSuccessStatusCode()
	{
		var httpClient = _factory.CreateClient();
		var isLoggedInUrl = "/api/user/isLoggedIn";

		var response = await httpClient.GetAsync(isLoggedInUrl);

		Assert.False(response.IsSuccessStatusCode);
	}

	[Fact]
	public async Task UserIsAbleToRegisterAndIsLoggedIn()
	{
		var httpClient = _factory.CreateClient();

		var registerResponse = await httpClient.PostAsJsonAsync(Routes.User.Register, UserInfos.User1.ToUserRegisterDto());
		var isLoggedInResponse = await httpClient.GetAsync(Routes.User.IsLoggedIn);

		Assert.True(registerResponse.IsSuccessStatusCode);
		Assert.True(isLoggedInResponse.IsSuccessStatusCode);
	}

	[Fact]
	public async Task UserIsAbleToLogout()
	{
		var httpClient = _factory.CreateClient();

		await httpClient.PostAsJsonAsync(Routes.User.Register, UserInfos.User2.ToUserRegisterDto());
		await httpClient.PostAsync(Routes.User.Logout, null);

		var isLoggedInResponse = await httpClient.GetAsync(Routes.User.IsLoggedIn);
		Assert.False(isLoggedInResponse.IsSuccessStatusCode);
	}

	[Fact]
	public async Task UserIsAbleToLoginWithCreatedAccount()
	{
		var httpClient = _factory.CreateClient();

		await httpClient.PostAsJsonAsync(Routes.User.Register, UserInfos.User3.ToUserRegisterDto());
		await httpClient.PostAsync(Routes.User.Logout, null);

		var loginResponse = await httpClient.PostAsJsonAsync(Routes.User.Login, UserInfos.User3.ToLoginDto());
		var isLoggedInResponse = await httpClient.GetAsync(Routes.User.IsLoggedIn);

		Assert.True(loginResponse.IsSuccessStatusCode);
		Assert.True(isLoggedInResponse.IsSuccessStatusCode);
	}
}

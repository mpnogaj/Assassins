using System.Net;
using Assassins.IntegrationTests.Consts;
using Assassins.IntegrationTests.Fixtures;
using Assassins.IntegrationTests.Utils.GameService;
using Assassins.Web.Dto;

namespace Assassins.IntegrationTests.Tests.GameService;

public class GameRegistrationTests : IClassFixture<WebAppWithRegisteredAndLoggedUsers>
{
	private readonly WebAppWithRegisteredAndLoggedUsers _app;

	public GameRegistrationTests(WebAppWithRegisteredAndLoggedUsers app)
	{
		_app = app;
	}

	[Fact]
	public async Task UserCanRegisterForGame()
	{
		var user = _app.LoggedInUsers.First();
		var userRegisteredBefore =
			await GameRegistrationUtils.IsUserRegistered(_app.Services, user.UserInfo.Username);
		var client = user.HttpClient;

		var response = await client.PostAsync(Routes.Game.Register, null);

		var isUserRegistered =
			await GameRegistrationUtils.IsUserRegistered(_app.Services, user.UserInfo.Username);
		Assert.False(userRegisteredBefore);
		Assert.True(response.IsSuccessStatusCode);
		Assert.True(isUserRegistered);
	}

	[Fact]
	public async Task UserCanUnregister()
	{
		var user = _app.LoggedInUsers.First();
		var client = user.HttpClient;

		await client.PostAsync(Routes.Game.Register, null);
		var response = await client.PostAsync(Routes.Game.Register, null);

		var isUserRegistered =
			await GameRegistrationUtils.IsUserRegistered(_app.Services, user.UserInfo.Username);
		Assert.True(response.IsSuccessStatusCode);
		Assert.False(isUserRegistered);
	}

	[Fact]
	public async Task UnauthorizedUserCannotRegisterForGame()
	{
		var user = _app.UnauthorizedUser;
		var client = user.HttpClient;

		var response = await client.PostAsync(Routes.Game.Register, null);

		var isUserRegistered =
			await GameRegistrationUtils.IsUserRegistered(_app.Services, user.UserInfo.Username);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(response.StatusCode, HttpStatusCode.Unauthorized);
		Assert.False(isUserRegistered);
	}

	[Fact]
	public async Task IsRegisteredEndpointReturnsCorrectData()
	{
		var user = _app.LoggedInUsers.First();
		var client = user.HttpClient;

		var isUserRegistered =
			await GameRegistrationUtils.IsUserRegistered(_app.Services, user.UserInfo.Username);

		var isRegisteredResponse = await client.GetAsync(Routes.Game.Register);
		var isRegisteredDto = await isRegisteredResponse.Content.ReadFromJsonAsync<RegistrationStatusDto>();

		Assert.True(isRegisteredResponse.IsSuccessStatusCode);
		Assert.True(isUserRegistered == (isRegisteredDto?.Registered ?? false));

		var registerResponse = await client.PostAsync(Routes.Game.Register, null);

		Assert.True(registerResponse.IsSuccessStatusCode);

		isUserRegistered = await GameRegistrationUtils.IsUserRegistered(_app.Services, user.UserInfo.Username);

		isRegisteredResponse = await client.GetAsync(Routes.Game.Register);
		isRegisteredDto = await isRegisteredResponse.Content.ReadFromJsonAsync<RegistrationStatusDto>();

		Assert.True(isRegisteredResponse.IsSuccessStatusCode);
		Assert.True(isUserRegistered == (isRegisteredDto?.Registered ?? false));
	}
}
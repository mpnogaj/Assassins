using Assassins.IntegrationTests.Consts;
using Assassins.IntegrationTests.Fixtures;
using Assassins.IntegrationTests.Utils.GameService;
using Assassins.IntegrationTests.Utils.User;
using Assassins.Web.Dto;

namespace Assassins.IntegrationTests.Tests.GameService;

public class GameInProgressTests : IClassFixture<WebAppWithRegisteredAndLoggedUsers>
{
	private readonly WebAppWithRegisteredAndLoggedUsers _app;

	public GameInProgressTests(WebAppWithRegisteredAndLoggedUsers app)
	{
		_app = app;
	}

	[Fact]
	public async Task GameStateIsInRegistrationStateByDefault()
	{
		var user = _app.LoggedInUsers.First();
		var client = user.HttpClient;
		var gameStateResult = await client.GetAsync(Routes.Game.GameState);
		var gameStateDto = await gameStateResult.Content.ReadFromJsonAsync<GameStateDto>();

		Assert.True(gameStateResult.IsSuccessStatusCode);
		Assert.Equal(GameStates.RegistrationState, gameStateDto?.GameState ?? string.Empty);
	}

	[Fact]
	public async Task GameSwitchesToWinStateWithOnlyOneParticipant()
	{
		await RegisterUserForGame(_app.LoggedInUsers.First());

		await _app.AdminUser.HttpClient.PostAsync(Routes.Admin.CloseRegistration, null);
		await _app.AdminUser.HttpClient.PostAsync(Routes.Admin.StartGame, null);

		var gameStateResult = await _app.LoggedInUsers.First().HttpClient.GetAsync(Routes.Game.GameState);
		var gameStateDto = await gameStateResult.Content.ReadFromJsonAsync<GameStateDto>();

		Assert.True(gameStateResult.IsSuccessStatusCode);
		Assert.Equal(gameStateDto?.GameState ?? string.Empty, GameStates.FinishedState);
	}

	private async Task RegisterUserForGame(UserContext user)
	{
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
}
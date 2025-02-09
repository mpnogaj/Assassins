using Assassins.Web;
using Assassins.Web.Dto;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Assassins.IntegrationTests;

public class UserRegistrationTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly WebApplicationFactory<Program> _factory;

	public UserRegistrationTests(WebApplicationFactory<Program> factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task IsLoggedInDoesNotSendSuccessStatusCode()
	{
		var httpClient = _factory.CreateClient();
		var isLoggedInUrl = "/api/user/isLoggedIn";

		var response = await httpClient.GetAsync(isLoggedInUrl);

		Assert.Throws<HttpRequestException>(response.EnsureSuccessStatusCode);
	}

	[Fact]
	public async Task UserIsAbleToLogIn()
	{
		var httpClient = _factory.CreateClient();
		var loginUrl = "/api/user/login";
		var loginPayload = new LoginDto
		{
			Username = "user1",
			Password = "pswd"
		};

		var loginResponse = await httpClient.PostAsJsonAsync(loginUrl, loginPayload);

		loginResponse.EnsureSuccessStatusCode();

		var isLoggedInUrl = "/api/user/isLoggedIn";

		var response = await httpClient.GetAsync(isLoggedInUrl);

		response.EnsureSuccessStatusCode();
	}
}

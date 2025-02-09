using Assassins.Web;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Assassins.IntegrationTests;

public class PingTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public PingTests(WebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task PingReturnsPong()
	{
		var pingUrl = "/api/ping";

		var response = await _client.GetAsync(pingUrl);
		var body = await response.Content.ReadAsStringAsync();


		response.EnsureSuccessStatusCode();
		Assert.Equal("pong", body);
	}
}

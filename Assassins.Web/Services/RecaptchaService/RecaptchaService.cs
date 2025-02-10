using System.Text.Json.Serialization;

namespace Assassins.Web.Services.RecaptchaService;

public class RecaptchaService : IRecaptchaService
{
	private readonly IConfiguration _configuration;
	private readonly IHttpClientFactory _httpClientFactory;

	private class RecaptchaRequestBody
	{
		[JsonPropertyName("secret")]
		public string Secret { get; set; } = null!;

		[JsonPropertyName("response")]
		public string Response { get; set; } = null!;

		[JsonPropertyName("remoteip")]
		public string? RemoteIp { get; set; } = null;
	}

	private class RecaptchaResponseBody
	{
		public bool Success { get; set; }
		[JsonPropertyName("challenge_ts")]
		public DateTime ChallengeTs { get; set; }
		public string Hostname { get; set; } = null!;
		[JsonPropertyName("error-codes")]
		public string[]? ErrorCodes { get; set; }
	}

	public RecaptchaService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		_configuration = configuration;
		_httpClientFactory = httpClientFactory;
	}

	public async Task<bool> ValidateRecaptcha(string token)
	{
		var recaptchaSecret = _configuration["Recaptcha:SecretKey"];
		if (recaptchaSecret == null)
		{
			return false;
		}

		var httpClient = _httpClientFactory.CreateClient();

		var validationResult = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("secret", recaptchaSecret),
			new KeyValuePair<string, string>("response", token)
		}));

		if (!validationResult.IsSuccessStatusCode)
		{
			return false;
		}

		var responseBody = await validationResult.Content.ReadFromJsonAsync<RecaptchaResponseBody>();
		return responseBody?.Success ?? false;
	}
}
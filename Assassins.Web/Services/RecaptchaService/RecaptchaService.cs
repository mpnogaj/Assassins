using System.Text.Json.Serialization;
using Assassins.Web.Utils;

namespace Assassins.Web.Services.RecaptchaService;

public class RecaptchaService : IRecaptchaService
{
	private readonly IConfiguration _configuration;
	private readonly IHttpClientFactory _httpClientFactory;

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

	public async Task<Result<RecaptchaServiceErrors>> ValidateRecaptcha(string token)
	{
		var recaptchaSecret = _configuration["Recaptcha:SecretKey"];
		if (recaptchaSecret == null)
		{
			return Result<RecaptchaServiceErrors>.Failure(RecaptchaServiceErrors.RecaptchaSecretMissingError);
		}

		var httpClient = _httpClientFactory.CreateClient();

		var verificationResult =
			await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify",
				new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("secret", recaptchaSecret),
			new KeyValuePair<string, string>("response", token)
		}));

		if (!verificationResult.IsSuccessStatusCode)
		{
			return Result<RecaptchaServiceErrors>.Failure(RecaptchaServiceErrors.VerificationApiReturnedNonSuccessStatusCode);
		}

		var verificationBody = await verificationResult.Content.ReadFromJsonAsync<RecaptchaResponseBody>();
		var verificationSuccess = verificationBody?.Success ?? false;

		return verificationSuccess
			? Result<RecaptchaServiceErrors>.Success()
			: Result<RecaptchaServiceErrors>.Failure(RecaptchaServiceErrors.VerificationFailed);
	}
}
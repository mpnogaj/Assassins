namespace Assassins.Web.Services.RecaptchaService;

public enum RecaptchaServiceErrors
{
	RecaptchaSecretMissingError,
	VerificationApiReturnedNonSuccessStatusCode,
	VerificationFailed
}
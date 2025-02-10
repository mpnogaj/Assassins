namespace Assassins.Web.Services.RecaptchaService;

public interface IRecaptchaService
{
	public Task<bool> ValidateRecaptcha(string token);
}
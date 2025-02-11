using Assassins.Web.Utils;

namespace Assassins.Web.Services.RecaptchaService;

public interface IRecaptchaService
{
	public Task<Result<RecaptchaServiceErrors>> ValidateRecaptcha(string token);
}
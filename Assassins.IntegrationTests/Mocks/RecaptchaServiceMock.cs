using Assassins.Web.Services.RecaptchaService;
using Assassins.Web.Utils;

namespace Assassins.IntegrationTests.Mocks;

public class RecaptchaServiceMock : IRecaptchaService
{
	public Task<Result<RecaptchaServiceErrors>> ValidateRecaptcha(string token)
		=> Task.FromResult(Result<RecaptchaServiceErrors>.Success());
}
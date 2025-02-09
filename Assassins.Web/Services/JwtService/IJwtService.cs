using Assassins.Web.Models;

namespace Assassins.Web.Services.JwtService;

public interface IJwtService
{
	public string CreateJwtToken(User user);
}
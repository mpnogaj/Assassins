using Assassins.Models;

namespace Assassins.Services.JwtService;

public interface IJwtService
{
	public string CreateJwtToken(User user);
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Assassins.Models;
using Microsoft.IdentityModel.Tokens;

namespace Assassins.Services.JwtService;

public class JwtService : IJwtService
{
	private readonly IConfiguration _configuration;

	public JwtService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string CreateJwtToken(User user)
	{
		return CreateJwtToken(new List<Claim>()
		{
			new(ClaimTypes.Name, $"{user.FirstName} ${user.LastName}"),
			new(ClaimTypes.NameIdentifier, user.Username),
			new(ClaimTypes.Role, user.IsAdmin ? Roles.Admin : Roles.User)
		});
	}

	private string CreateJwtToken(IEnumerable<Claim> claims)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]!));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


		var jwtToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(2),
			signingCredentials: credentials);

		var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

		return token;
	}
}
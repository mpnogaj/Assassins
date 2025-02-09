using Assassins.Web.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Assassins.Web;

public static class Extensions
{
	public static List<UserRegisterDto> GetAdminUsers(this IConfiguration configuration)
	{
		return configuration.ExtractUsersFromConfigSection("AdminUsers");
	}

	public static List<UserRegisterDto> GetNormalUsers(this IConfiguration configuration)
	{
		return configuration.ExtractUsersFromConfigSection("NormalUsers");
	}

	public static List<UserRegisterDto> ExtractUsersFromConfigSection(this IConfiguration configuration, string sectionName)
	{
		var usersSection = configuration.GetSection(sectionName);

		return usersSection.GetChildren().Select(userSection =>
		{
			var firstName = userSection["FirstName"];
			var lastName = userSection["LastName"];
			var username = userSection["Username"];
			var password = userSection["Password"];

			if (firstName == null || lastName == null || username == null || password == null)
			{
				return null;
			}

			return new UserRegisterDto
			{
				FirstName = firstName,
				LastName = lastName,
				Username = username,
				Password = password
			};
		}).NotNull().ToList();
	}
}

public static class RandomExtensions
{
	private static readonly Random _rng = new Random();

	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
	{
		return list.OrderBy(_ => _rng.Next());
	}

	public static string RandomAlphaNumericString(int length)
	{
		const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		return new string(Enumerable.Repeat(alphabet, length)
									.Select(s => s[_rng.Next(s.Length)]).ToArray());
	}
}

public static class LinqExtensions
{
	public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
	{
		return enumerable.Where(e => e != null).Select(e => e!);
	}
}
using Assassins.Dto;

namespace Assassins;

public static class Extensions
{
	public static List<UserRegisterDto> GetAdminUsers(this IConfiguration configuration)
	{
		var adminUsersSection = configuration.GetSection("AdminUsers");

		return adminUsersSection.GetChildren().Select(adminUserSection =>
		{
			var firstName = adminUserSection["FirstName"];
			var lastName = adminUserSection["LastName"];
			var username = adminUserSection["Username"];
			var password = adminUserSection["Password"];

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
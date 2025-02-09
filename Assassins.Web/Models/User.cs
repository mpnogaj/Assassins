using Assassins.Web.Dto;

namespace Assassins.Web.Models
{
	/// <summary>
	/// Represents user with created account
	/// </summary>
	public class User
	{
		public User()
		{

		}

		public User(UserRegisterDto userRegisterDto, string passwordHash, bool isAdmin = false)
		{
			Id = Guid.NewGuid();
			Username = userRegisterDto.Username;
			FirstName = userRegisterDto.FirstName;
			LastName = userRegisterDto.LastName;
			PasswordHash = passwordHash;

			IsAdmin = isAdmin;
		}

		public Guid Id { get; set; }
		public string Username { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;

		public bool IsAdmin { get; init; } = false;

		public bool Registered { get; set; } = false;

		public string FullName => $"{FirstName} {LastName}";
	}
}

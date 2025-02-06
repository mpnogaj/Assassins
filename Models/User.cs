using Assassins.Dto;

namespace Assassins.Models
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
			this.Id = Guid.NewGuid();
			this.Username = userRegisterDto.Username;
			this.FirstName = userRegisterDto.FirstName;
			this.LastName = userRegisterDto.LastName;
			this.PasswordHash = passwordHash;

			this.IsAdmin = isAdmin;
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

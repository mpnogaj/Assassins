namespace Assassins.Web.Dto;

public class UserRegisterDto
{
	public string Username { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Password { get; set; } = null!;
}
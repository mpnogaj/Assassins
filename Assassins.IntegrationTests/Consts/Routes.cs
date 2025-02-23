namespace Assassins.IntegrationTests.Consts;

public static class Routes
{
	public static class User
	{
		public const string IsLoggedIn = "/api/user/isLoggedIn";
		public const string Register = "/api/user/register";
		public const string Login = "/api/user/login";
		public const string Logout = "/api/user/logout";
	}

	public static class Game
	{
		public const string Register = "/api/game/register";
		public const string GameState = "/api/game/state";
	}

	public static class Admin
	{
		public const string CloseRegistration = "/api/admin/closeRegistration";
		public const string StartGame = "/api/admin/startGame";
	}
}
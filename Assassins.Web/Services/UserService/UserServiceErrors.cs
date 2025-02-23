using Assassins.Web.Utils;

namespace Assassins.Web.Services.UserService;

public enum UserServiceErrors
{
	UsernameNotFoundError,
	InvalidPasswordError,
	UsernameTakenError,
	PasswordTooShortError
}
using Assassins.Web.Dto;
using Assassins.Web.Models;
using Assassins.Web.Repositories.UserRepository;
using Assassins.Web.Utils;
using BC = BCrypt.Net.BCrypt;

namespace Assassins.Web.Services.UserService;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result<User, UserServiceErrors>> GetUser(string username)
	{
		var user = await _userRepository.GetUser(username);
		if (user == null)
		{
			return Result<User, UserServiceErrors>.Failure(UserServiceErrors.UsernameNotFoundError);
		}

		return Result<User, UserServiceErrors>.Success(user);
	}

	public async Task<Result<User, UserServiceErrors>> Login(LoginDto loginDetails)
	{
		var user = await _userRepository.GetUser(loginDetails.Username);
		if (user == null)
		{
			return Result<User, UserServiceErrors>.Failure(UserServiceErrors.UsernameNotFoundError);
		}

		if (!BC.Verify(loginDetails.Password, user.PasswordHash))
		{
			return Result<User, UserServiceErrors>.Failure(UserServiceErrors.InvalidPasswordError);
		}

		return Result<User, UserServiceErrors>.Success(user);
	}

	public async Task<Result<User, UserServiceErrors>> Register(UserRegisterDto userRegisterDetails)
	{
		if (userRegisterDetails.Password.Length < 8)
		{
			return Result<User, UserServiceErrors>.Failure(UserServiceErrors.PasswordTooShortError);
		}

		var userAlreadyExists = await IsUsernameAlreadyInUse(userRegisterDetails.Username);

		if (userAlreadyExists)
		{
			return Result<User, UserServiceErrors>.Failure(UserServiceErrors.UsernameTakenError);
		}

		var user = new User(userRegisterDetails, BC.HashPassword(userRegisterDetails.Password));
		await _userRepository.CreateUser(user);
		return Result<User, UserServiceErrors>.Success(user);
	}

	private async Task<bool> IsUsernameAlreadyInUse(string username)
	{
		return (await _userRepository.GetUsers()).Any(u => u.Username == username);
	}
}
using Assassins.Dto;
using Assassins.Models;
using Assassins.Services.Repositories.UserRepository;

using BC = BCrypt.Net.BCrypt;

namespace Assassins.Services.UserService;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public Task<User?> GetUser(string username)
	{
		return _userRepository.GetUser(username);
	}

	public async Task<User?> Login(LoginDto loginDetails)
	{
		//TODO: add dto validation here
		var user = await _userRepository.GetUser(loginDetails.Username);
		if (user == null)
		{
			return null;
		}

		if (!BC.Verify(loginDetails.Password, user.PasswordHash))
		{
			return null;
		}

		return user;
	}

	public async Task<User?> Register(UserRegisterDto userRegisterDetails)
	{
		//TODO: add dto validation here
		var user = new User(userRegisterDetails, BC.HashPassword(userRegisterDetails.Password));
		await _userRepository.CreateUser(user);
		return user;
	}
}
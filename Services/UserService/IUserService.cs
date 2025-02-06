using Assassins.Dto;
using Assassins.Models;

namespace Assassins.Services.UserService;

public interface IUserService
{
	public Task<User?> GetUser(string username);
	public Task<User?> Login(LoginDto loginDetails);
	public Task<User?> Register(UserRegisterDto userRegisterDetails);
}
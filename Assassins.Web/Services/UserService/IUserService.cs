using Assassins.Web.Dto;
using Assassins.Web.Models;

namespace Assassins.Web.Services.UserService;

public interface IUserService
{
	public Task<User?> GetUser(string username);
	public Task<User?> Login(LoginDto loginDetails);
	public Task<User?> Register(UserRegisterDto userRegisterDetails);
}
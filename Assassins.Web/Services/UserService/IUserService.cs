using Assassins.Web.Dto;
using Assassins.Web.Models;
using Assassins.Web.Utils;

namespace Assassins.Web.Services.UserService;

public interface IUserService
{
	public Task<Result<User, UserServiceErrors>> GetUser(string username);
	public Task<Result<User, UserServiceErrors>> Login(LoginDto loginDetails);
	public Task<Result<User, UserServiceErrors>> Register(UserRegisterDto userRegisterDetails);
}
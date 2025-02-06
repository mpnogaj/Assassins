using Assassins.Models;

namespace Assassins.Services.Repositories.UserRepository;

public interface IUserRepository
{
	public Task<List<User>> GetUsers();
	public Task<User?> GetUser(string username);

	public Task<List<User>> GetRegisteredUsers();

	public Task CreateUser(User user);

	public Task UpdateUsers(List<User> users);
	public Task UpdateUser(User user);
}
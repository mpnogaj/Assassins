using Assassins.Db;
using Assassins.Models;
using Microsoft.EntityFrameworkCore;

namespace Assassins.Services.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
	private readonly AppDbContext _dbContext;

	public UserRepository(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<List<User>> GetUsers()
	{
		return _dbContext.Users.ToListAsync();
	}

	public Task<User?> GetUser(string username)
	{
		return _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
	}

	public Task<List<User>> GetRegisteredUsers()
	{
		return _dbContext.Users.Where(user => user.Registered && !user.IsAdmin).ToListAsync();
	}

	public Task CreateUser(User user)
	{
		_dbContext.Users.Add(user);
		return _dbContext.SaveChangesAsync();
	}

	public Task UpdateUsers(List<User> users)
	{
		foreach (var user in users)
		{
			_dbContext.Users.Attach(user);
			_dbContext.Entry(user).State = EntityState.Modified;
		}

		return _dbContext.SaveChangesAsync();
	}

	public Task UpdateUser(User user)
	{
		_dbContext.Users.Attach(user);
		_dbContext.Entry(user).State = EntityState.Modified;

		return _dbContext.SaveChangesAsync();
	}
}
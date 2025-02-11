using Assassins.Web.Db;
using Assassins.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Assassins.Web.Services.Repositories.UserRepository;

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

	public async Task<List<User>> GetRegisteredUsers()
	{
		return (await GetUsers()).Where(user => user.Registered).ToList();
	}

	public Task CreateUser(User user)
	{
		_dbContext.Users.Add(user);
		return _dbContext.SaveChangesAsync();
	}

	public async Task UpdateUsers(List<User> users)
	{
		foreach (var user in users)
		{
			await UpdateUserHelper(user);
		}

		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateUser(User user)
	{
		await UpdateUserHelper(user);
		await _dbContext.SaveChangesAsync();
	}

	private async Task UpdateUserHelper(User user)
	{
		var entry = _dbContext.Entry(user);
		if (entry.State == EntityState.Detached)
		{
			var existingEntity = await GetUser(user.Username);
			if (existingEntity == null)
			{
				return;
			}

			_dbContext.Entry(existingEntity).CurrentValues.SetValues(user);
		}

		_dbContext.Users.Update(user);
	}
}
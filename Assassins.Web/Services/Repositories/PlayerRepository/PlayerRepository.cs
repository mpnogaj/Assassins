using Assassins.Web.Db;
using Assassins.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Assassins.Web.Services.Repositories.PlayerRepository;

public class PlayerRepository : IPlayerRepository
{
	private readonly AppDbContext _dbContext;

	public PlayerRepository(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<Player?> GetPlayer(Guid id)
	{
		return _dbContext.Players
						 .Include(player => player.User)
						 .FirstOrDefaultAsync(player => player.Id == id);
	}

	public Task<List<Player>> GetPlayers()
	{
		return _dbContext.Players
						 .Include(player => player.User)
						 .ToListAsync();
	}

	public async Task UpdatePlayers(List<Player> players)
	{
		await Task.WhenAll(players.Select(UpdatePlayerHelper));
		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdatePlayer(Player player)
	{
		await UpdatePlayerHelper(player);
		await _dbContext.SaveChangesAsync();
	}

	private async Task UpdatePlayerHelper(Player player)
	{
		var entry = _dbContext.Entry(player);
		if (entry.State == EntityState.Detached)
		{
			var existingEntity = await GetPlayer(player.Id);
			if (existingEntity == null)
			{
				return;
			}

			_dbContext.Entry(existingEntity).CurrentValues.SetValues(player);
		}

		_dbContext.Players.Update(player);
	}

	public Task DeleteAllPlayers()
	{
		try
		{
			return _dbContext.Players.ExecuteDeleteAsync();
		}
		catch (InvalidOperationException)
		{
			// in memory provider doesn't support ExecuteDeleteAsync. As fallback use this
			_dbContext.Players.RemoveRange(_dbContext.Players);
			return _dbContext.SaveChangesAsync();
		}
	}

	public async Task AddPlayers(List<Player> players)
	{
		await _dbContext.Players.AddRangeAsync(players);
		await _dbContext.SaveChangesAsync();
	}
}
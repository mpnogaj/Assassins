using Assassins.Web.Db;
using Assassins.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Assassins.Web.Repositories.PlayerRepository;

public class PlayerRepository(AppDbContext dbContext) : IPlayerRepository
{
	public Task<Player?> GetPlayer(Guid id)
	{
		return dbContext.Players
						 .Include(player => player.User)
						 .FirstOrDefaultAsync(player => player.Id == id);
	}

	public Task<List<Player>> GetPlayers()
	{
		return dbContext.Players
						 .Include(player => player.User)
						 .ToListAsync();
	}

	public async Task UpdatePlayers(List<Player> players)
	{
		await Task.WhenAll(players.Select(UpdatePlayerHelper));
		await dbContext.SaveChangesAsync();
	}

	public async Task UpdatePlayer(Player player)
	{
		await UpdatePlayerHelper(player);
		await dbContext.SaveChangesAsync();
	}

	private async Task UpdatePlayerHelper(Player player)
	{
		var entry = dbContext.Entry(player);
		if (entry.State == EntityState.Detached)
		{
			var existingEntity = await GetPlayer(player.Id);
			if (existingEntity == null)
			{
				return;
			}

			dbContext.Entry(existingEntity).CurrentValues.SetValues(player);
		}

		dbContext.Players.Update(player);
	}

	public Task DeleteAllPlayers()
	{
		try
		{
			return dbContext.Players.ExecuteDeleteAsync();
		}
		catch (InvalidOperationException)
		{
			// in memory provider doesn't support ExecuteDeleteAsync. As fallback use this
			dbContext.Players.RemoveRange(dbContext.Players);
			return dbContext.SaveChangesAsync();
		}
	}

	public async Task AddPlayers(List<Player> players)
	{
		await dbContext.Players.AddRangeAsync(players);
		await dbContext.SaveChangesAsync();
	}
}
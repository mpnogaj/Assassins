using Assassins.Db;
using Assassins.Models;
using Microsoft.EntityFrameworkCore;

namespace Assassins.Services.Repositories.PlayerRepository;

public class PlayerRepository : IPlayerRepository
{
	private readonly AppDbContext _dbContext;

	public PlayerRepository(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<Player?> GetPlayer(Guid id)
	{
		return _dbContext.Players.Include(player => player.User).FirstOrDefaultAsync(player => player.Id == id);
	}

	public Task<List<Player>> GetPlayers()
	{
		return _dbContext.Players.Include(player => player.User).ToListAsync();
	}

	public Task<List<Player>> GetPlayersWithVictims()
	{
		return _dbContext.Players
		                 .Include(player => player.User)
		                 .Include(player => player.Target)
		                 .ToListAsync();
	}

	public async Task UpdatePlayers(List<Player> players)
	{
		foreach (var player in players)
		{
			var entry = _dbContext.Entry(player);
			if (entry.State != EntityState.Detached) continue;

			var attachedPlayer = await _dbContext.Players.FindAsync(player.Id);
			if (attachedPlayer != null)
			{
				var attachedEntry = _dbContext.Entry(player);
				attachedEntry.CurrentValues.SetValues(player);
			}
			else
			{
				entry.State = EntityState.Modified;
			}
		}
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
using Assassins.Models;

namespace Assassins.Services.Repositories.PlayerRepository;

public interface IPlayerRepository
{
	public Task<Player?> GetPlayer(Guid id);
	public Task<List<Player>> GetPlayers();

	public Task UpdatePlayers(List<Player> players);
	public Task UpdatePlayer(Player player);

	public Task DeleteAllPlayers();
	public Task AddPlayers(List<Player> players);
}
using Assassins.Models;

namespace Assassins.Services.GameService;

public interface IGameService
{
	public GameState GameState { get; }

	public Task OpenRegistration();
	public Task CloseRegistration();
	public Task StartGame();

	/// <summary>
	/// Returns player with given user account
	/// Returns null when user doesn't take part in game
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	public Task<Player?> GetPlayer(User user);
	public Task<bool> ToggleRegisterUser(User user);
	public Task<bool> AttemptKill(Player killer, string code);

	public Task<List<Player>> GetPlayersWithTargets();
}
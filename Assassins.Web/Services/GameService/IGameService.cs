using Assassins.Web.Models;
using Assassins.Web.Services.GameService.GameServiceErrors;
using Assassins.Web.Utils;

namespace Assassins.Web.Services.GameService;

public interface IGameService
{
	public GameState GameState { get; }

	public Task OpenRegistration();
	public Task CloseRegistration();
	public Task StartGame();

	public Task<Result<List<User>, GetRegisteredUsersErrors>> GetRegisteredUsers();
	public Task<Result<KickUserErrors>> KickUser(Guid userId);

	public Task<Result<Player, GetPlayerErrors>> GetPlayer(User user);
	public Task<Result<Player, GetTargetErrors>> GetTarget(Player player);
	public Result<User, GetWinnerErrors> GetWinner();

	public Task<Result<ToggleRegisterUserErrors>> ToggleRegisterUser(User user);
	public Task<Result<KillErrors>> AttemptKill(Player killer, string code);
	public Task<Result<KillErrors>> AdminKill(Guid killerGuid);

	public Task<List<(Player player, Player? target)>> GetPlayersWithTargets();
}
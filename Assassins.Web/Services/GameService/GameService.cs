using Assassins.Web.Hub;
using Assassins.Web.Models;
using Assassins.Web.Services.GameService.GameServiceErrors;
using Assassins.Web.Services.Repositories.PlayerRepository;
using Assassins.Web.Services.Repositories.UserRepository;
using Assassins.Web.Utils;
using Microsoft.AspNetCore.SignalR;

namespace Assassins.Web.Services.GameService;

public class GameService : IGameService
{
	private readonly IUserRepository _userRepository;
	private readonly IPlayerRepository _playerRepository;
	private readonly IHubContext<AssassinsHub, IAssassinsClient> _hubContext;

	private static GameState _gameState = new UnknownState();

	public GameState GameState
	{
		get => _gameState;
		set
		{
			if (value == _gameState) return;
			_gameState = value;

			_hubContext.Clients.All.NotifyGameStateChanged();
		}
	}

	public GameService(IUserRepository userRepository, IPlayerRepository playerRepository, IHubContext<AssassinsHub, IAssassinsClient> hubContext)
	{
		_userRepository = userRepository;
		_playerRepository = playerRepository;
		_hubContext = hubContext;
	}

	public async Task OpenRegistration()
	{
		var users = await _userRepository.GetUsers();
		users.ForEach(user =>
		{
			user.Registered = false;
		});
		await _userRepository.UpdateUsers(users);
		GameState = new RegistrationState();
	}

	public async Task CloseRegistration()
	{
		var registeredUsers = await _userRepository.GetRegisteredUsers();
		GameState = new AboutToStartState(registeredUsers);
	}

	public async Task StartGame()
	{
		if (GameState is not AboutToStartState _)
		{
			return;
		}

		var registeredUsers = await _userRepository.GetRegisteredUsers();

		var shuffledPlayers = registeredUsers.Shuffle().Select(user => new Player
		{
			Id = Guid.NewGuid(),
			User = user
		}).ToList();

		for (var i = 0; i < shuffledPlayers.Count; i++)
		{
			shuffledPlayers[i].TargetGuid = shuffledPlayers[(i + 1) % shuffledPlayers.Count].Id;
			shuffledPlayers[i].KillCode = RandomExtensions.RandomAlphaNumericString(8);
		}

		await _playerRepository.DeleteAllPlayers();
		await _playerRepository.AddPlayers(shuffledPlayers);

		GameState = shuffledPlayers.Count switch
		{
			0 => new RegistrationState(),
			1 => new FinishedState(shuffledPlayers.First().User),
			_ => new InProgressState(shuffledPlayers.Count, shuffledPlayers.Count)
		};
	}

	public async Task<Result<Player, GetPlayerErrors>> GetPlayer(User user)
	{
		if (GameState is not (InProgressState or FinishedState))
		{
			return Result<Player, GetPlayerErrors>.Failure(GetPlayerErrors.InvalidGameStateError);
		}

		var players = await _playerRepository.GetPlayers();
		var player = players.FirstOrDefault(player => player.User.Id == user.Id);

		return player != null
			? Result<Player, GetPlayerErrors>.Success(player)
			: Result<Player, GetPlayerErrors>.Failure(GetPlayerErrors.UserDoesNotTakePartInGameError);
	}

	public async Task<Result<Player, GetTargetErrors>> GetTarget(Player player)
	{
		var target = await _playerRepository.GetPlayer(player.TargetGuid);
		return target != null
			? Result<Player, GetTargetErrors>.Success(target)
			: Result<Player, GetTargetErrors>.Failure(GetTargetErrors.TargetUserNotFound);
	}

	public Result<User, GetWinnerErrors> GetWinner()
	{
		return GameState is not FinishedState finishedState
			? Result<User, GetWinnerErrors>.Failure(GetWinnerErrors.InvalidGameStateError)
			: Result<User, GetWinnerErrors>.Success(finishedState.Winner);
	}

	public async Task<Result<ToggleRegisterUserErrors>> ToggleRegisterUser(User user)
	{
		if (GameState is not RegistrationState)
		{
			return Result<ToggleRegisterUserErrors>.Failure(ToggleRegisterUserErrors.NotInRegistrationStateError);
		}

		user.Registered = !user.Registered;
		await _userRepository.UpdateUser(user);

		return Result<ToggleRegisterUserErrors>.Success();
	}

	public async Task<Result<KillErrors>> AttemptKill(Player killer, string code)
	{
		if (GameState is not InProgressState _)
		{
			return Result<KillErrors>.Failure(KillErrors.GameIsNotInProgressError);
		}

		return await KillPlayer(killer, code);
	}

	public async Task<Result<KillErrors>> AdminKill(Guid killerGuid)
	{
		if (GameState is not InProgressState _)
		{
			return Result<KillErrors>.Failure(KillErrors.GameIsNotInProgressError);
		}

		var killer = await _playerRepository.GetPlayer(killerGuid);
		if (killer == null)
		{
			return Result<KillErrors>.Failure(KillErrors.KillerNotFound);
		}

		var getTargetResult = await GetTarget(killer);

		return await getTargetResult.MatchAsync(
			onSuccess: async (target) => await KillPlayer(killer, target.KillCode),
			onFailure: (_) => Result<KillErrors>.Failure(KillErrors.TargetNotFound));
	}

	public async Task<List<(Player player, Player? target)>> GetPlayersWithTargets()
	{
		if (GameState is not InProgressState)
		{
			throw new InvalidOperationException("Game must be in progress when trying to get players with victims!");
		}

		var players = await _playerRepository.GetPlayers();

		var playersMap = players.ToDictionary(player => player.Id);

		var playersWithTargets = players
								 .Select(player => (player, target: playersMap.GetValueOrDefault(player.TargetGuid)))
								 .ToList();

		return playersWithTargets;
	}

	private async Task<Result<KillErrors>> KillPlayer(Player killer, string killCode)
	{
		if (GameState is not InProgressState inProgressState)
		{
			return Result<KillErrors>.Failure(KillErrors.GameIsNotInProgressError);
		}

		var getTargetResult = await GetTarget(killer);

		return await getTargetResult.MatchAsync(
			onSuccess: async (target) =>
			{
				if (target.KillCode != killCode)
				{
					return Result<KillErrors>.Failure(KillErrors.InvalidKillCode);
				}

				killer.TargetGuid = target.TargetGuid;

				await _playerRepository.UpdatePlayer(killer);

				target.Alive = false;
				target.TargetGuid = Guid.Empty;

				await _playerRepository.UpdatePlayer(target);

				var alivePlayers = (await _playerRepository.GetPlayers()).Count(player => player.Alive);

				GameState = new InProgressState(alivePlayers, inProgressState.TotalPlayers);

				if (alivePlayers == 1)
				{
					FinishGame(killer.User);
					return Result<KillErrors>.Success();
				}

				_ = _hubContext.Clients.All.NotifyKillHappened();

				return Result<KillErrors>.Success();
			},
			(_) => Result<KillErrors>.Failure(KillErrors.TargetNotFound));
	}

	private void FinishGame(User winner)
	{
		GameState = new FinishedState(winner);
	}
}
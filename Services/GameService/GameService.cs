using Assassins.Hub;
using Assassins.Models;
using Assassins.Services.Repositories.PlayerRepository;
using Assassins.Services.Repositories.UserRepository;
using Microsoft.AspNetCore.SignalR;

namespace Assassins.Services.GameService;

public class GameService : IGameService
{
	private readonly IServiceProvider _serviceProvider;
	private GameState _gameState;

	public GameState GameState
	{
		get => _gameState;
		set
		{
			if (value == _gameState) return;
			_gameState = value;

			GetServiceFromScope<IHubContext<AssassinsHub, IAssassinsClient>>(hub =>
			{
				hub.Clients.All.NotifyGameStateChanged();
			});
		}
	}

	public GameService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_gameState = new UnknownState();

		OpenRegistration();
	}

	public Task OpenRegistration()
	{
		return GetServiceFromScope<IUserRepository>(async userRepository =>
		{
			var users = await userRepository.GetUsers();
			users.ForEach(user =>
			{
				user.Registered = false;
			});
			await userRepository.UpdateUsers(users);
			GameState = new RegistrationState();
		});
	}

	public Task CloseRegistration()
	{
		return GetServiceFromScope<IUserRepository>(async userRepository =>
		{
			var registeredUsers = await userRepository.GetRegisteredUsers();
			GameState = new AboutToStartState(registeredUsers);
		});
	}

	public async Task StartGame()
	{
		using var scope = _serviceProvider.CreateScope();
		var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
		var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

		if (GameState is not AboutToStartState _)
		{
			return;
		}

		var registeredUsers = await userRepository.GetRegisteredUsers();

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

		await playerRepository.DeleteAllPlayers();
		await playerRepository.AddPlayers(shuffledPlayers);

		GameState = shuffledPlayers.Count switch
		{
			0 => new RegistrationState(),
			1 => new FinishedState(shuffledPlayers.First().User),
			_ => new InProgressState(shuffledPlayers.Count)
		};
	}

	public Task<Player?> GetPlayer(User user)
	{
		return GetServiceFromScope<Player?, IPlayerRepository>(async playerRepository =>
		{
			var players = await playerRepository.GetPlayers();
			var player = players.FirstOrDefault(player => player.User.Id == user.Id);
			return player;
		});
	}

	public Task<bool> ToggleRegisterUser(User user)
	{
		return GetServiceFromScope<bool, IUserRepository>(async userRepository =>
		{
			if (GameState is not RegistrationState) return false;

			user.Registered = !user.Registered;
			await userRepository.UpdateUser(user);
			return true;
		});
	}

	public Task<bool> AttemptKill(Player killer, string code)
	{
		return GetServiceFromScope<bool, IPlayerRepository>(async playerRepository =>
		{
			if (GameState is not InProgressState inProgressState || killer.Target.KillCode != code)
			{
				return false;
			}

			var killedPlayer = killer.Target;

			killedPlayer.Alive = false;

			killer.Target = killedPlayer.Target;
			killedPlayer.Target = killedPlayer;

			await playerRepository.UpdatePlayers(new List<Player> { killer, killedPlayer });

			var alivePlayers = (await playerRepository.GetPlayers()).Count(player => player.Alive);

			GameState = new InProgressState(alivePlayers);

			if (alivePlayers == 1)
			{
				FinishGame(killer.User);
			}

			return true;
		});
	}

	public Task<List<Player>> GetPlayersWithTargets()
	{
		if (GameState is not InProgressState)
		{
			throw new InvalidOperationException("Game must be in progress when trying to get players with victims!");
		}

		using var scope = _serviceProvider.CreateScope();
		var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
		return playerRepository.GetPlayersWithVictims();
	}

	private void FinishGame(User winner)
	{
		GameState = new FinishedState(winner);
	}

	private void GetServiceFromScope<TService>(Action<TService> callback) where TService : notnull
	{
		using var scope = _serviceProvider.CreateScope();
		var service = scope.ServiceProvider.GetRequiredService<TService>();
		callback(service);
	}

	private Task GetServiceFromScope<TService>(Func<TService, Task> callback) where TService : notnull
	{
		using var scope = _serviceProvider.CreateScope();
		var service = scope.ServiceProvider.GetRequiredService<TService>();
		return callback(service);
	}

	private Task<T> GetServiceFromScope<T, TService>(Func<TService, Task<T>> callback) where TService : notnull
	{
		using var scope = _serviceProvider.CreateScope();
		var service = scope.ServiceProvider.GetRequiredService<TService>();
		return callback(service);
	}
}
using Assassins.Web.Dto;
using Assassins.Web.Services.GameService;
using Assassins.Web.Services.GameService.GameServiceErrors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assassins.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
	private readonly IGameService _gameService;

	public AdminController(IGameService gameService)
	{
		_gameService = gameService;
	}

	[HttpGet("isAdmin")]
	public IActionResult IsAdmin()
	{
		return Ok();
	}

	[HttpPost("closeRegistration")]
	public async Task<IActionResult> CloseRegistration()
	{
		if (_gameService.GameState is not RegistrationState)
		{
			return Conflict();
		}

		await _gameService.CloseRegistration();
		return Ok();
	}

	[HttpPost("startGame")]
	public async Task<IActionResult> StartGame()
	{
		if (_gameService.GameState is not AboutToStartState)
		{
			return Conflict();
		}

		await _gameService.StartGame();
		return Ok();
	}

	[HttpPost("restartGame")]
	public async Task<IActionResult> RestartGame()
	{
		await _gameService.OpenRegistration();
		return Ok();
	}

	[HttpGet("registeredUsers")]
	public async Task<IActionResult> GetRegisteredUsers()
	{
		var registeredUsersResult = await _gameService.GetRegisteredUsers();

		return registeredUsersResult.Match<IActionResult>(
			onSuccess: (users) => Ok(new ParticipantsDto
			{
				Participants = users.Select(user => new ParticipantsDto.UserInfoDto()
				{
					Id = user.Id,
					FullName = user.FullName
				}).ToList()
			}),
			onFailure: (error) => error switch
			{
				GetRegisteredUsersErrors.InvalidGameState => Conflict("Invalid game state"),
				_ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
			});
	}

	[HttpPost("kickUser")]
	public async Task<IActionResult> KickUser([FromBody] KickUserDto kickUserDto)
	{
		var kickUserResult = await _gameService.KickUser(kickUserDto.UserId);
		return kickUserResult.Match<IActionResult>(
			onSuccess: () => Ok(),
			onFailure: (error) => error switch
			{
				KickUserErrors.UserWithGivenIdNotFound => NotFound("User not found"),
				_ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
			}
		);
	}

	[HttpGet("extendedProgress")]
	public async Task<IActionResult> GetExtendedGameProgress()
	{
		if (_gameService.GameState is not InProgressState inProgressState)
		{
			return Conflict();
		}

		var alivePlayers = inProgressState.AlivePlayers;
		var playersWithTargets = await _gameService.GetPlayersWithTargets();

		var playerData = playersWithTargets
						 .Select(playerWithTarget => new ExtendedGameProgressDto.PlayerWithTargetDto
						 {
							 Alive = playerWithTarget.target != null,
							 PlayerId = playerWithTarget.player.Id,
							 PlayerFullName = playerWithTarget.player.User.FullName,
							 VictimId = playerWithTarget.target?.Id,
							 VictimFullName = playerWithTarget.target?.User.FullName
						 }).ToList();

		var extendedGameProgressDto = new ExtendedGameProgressDto()
		{
			AliveUsers = alivePlayers,
			PlayerData = playerData
		};

		return Ok(extendedGameProgressDto);
	}

	[HttpPost("kill")]
	public async Task<IActionResult> AdminKill([FromBody] AdminKillDto adminKillDto)
	{
		var killResult = await _gameService.AdminKill(adminKillDto.PlayerGuid);

		return killResult.Match<IActionResult>(
			onSuccess: () => Ok(),
			onFailure: (error) => error switch
			{
				KillErrors.KillerNotFound => NotFound("Killer not found"),
				KillErrors.TargetNotFound => NotFound("Target not found"),
				KillErrors.GameIsNotInProgressError => Conflict("Invalid game state"),
				KillErrors.InvalidKillCode =>
					StatusCode(StatusCodes.Status500InternalServerError,
						"Invalid kill code when admin kill. Contact admin, this should happen"),
				_ => Problem("An unknown error occurred", statusCode: StatusCodes.Status500InternalServerError)
			});
	}
}
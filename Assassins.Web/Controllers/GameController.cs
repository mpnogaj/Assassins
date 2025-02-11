using Assassins.Web.Dto;
using Assassins.Web.Middlewares;
using Assassins.Web.Models;
using Assassins.Web.Services.GameService;
using Assassins.Web.Services.GameService.GameServiceErrors;
using Assassins.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Assassins.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GameController : ControllerBase
{
	private readonly IGameService _gameService;

	public GameController(IGameService gameService)
	{
		_gameService = gameService;
	}

	[HttpPost("register")]
	public Task<IActionResult> ToggleRegister()
	{
		if (_gameService.GameState is not RegistrationState _)
		{
			return Task.FromResult<IActionResult>(Conflict("Invalid game state"));
		}

		return HttpContext.GetLoggedUser().MatchAsync(
			onSuccess: async (user) =>
			{
				var toggleRegisterResult = await _gameService.ToggleRegisterUser(user);

				return toggleRegisterResult.Match<IActionResult>(
					onSuccess: () => Ok(),
					onFailure: (_) => Forbid());
			},
			onFailure: (_) => Unauthorized());
	}

	[HttpGet("register")]
	public IActionResult IsRegistered()
	{
		return HttpContext.GetLoggedUser().Match<IActionResult>(
			onSuccess: (user) => Ok(new RegistrationStatusDto
			{
				Registered = user.Registered
			}),
			onFailure: (_) => Unauthorized());
	}

	[HttpPost("kill")]
	[EnableRateLimiting("fixed")]
	public async Task<IActionResult> Kill([FromBody] KillRequestDto killRequestDto)
	{
		var getLoggedPlayerResult = await GetLoggedPlayer();

		return await getLoggedPlayerResult.MatchAsync(
			onSuccess: async (player) =>
			{
				var killResult = await _gameService.AttemptKill(player, killRequestDto.KillCode);
				return killResult.Match<IActionResult>(
					onSuccess: () => Ok(),
					onFailure: error => error switch
					{
						KillErrors.GameIsNotInProgressError => Conflict("Invalid game state"),
						KillErrors.KillerNotFound => NotFound("Killer not found"),
						KillErrors.TargetNotFound => NotFound("Target not found"),
						KillErrors.InvalidKillCode => StatusCode(StatusCodes.Status403Forbidden, "Invalid kill code"),
						_ => Problem("An unknown error occurred", statusCode: StatusCodes.Status500InternalServerError)
					});
			},
			onFailure: (_) => Forbid());
	}

	[HttpGet("state")]
	public IActionResult GetGameState()
	{
		var gameState = _gameService.GameState;
		return Ok(new GameStateDto { GameState = gameState.Name });
	}

	[HttpGet("progress")]
	[Authorize]
	public IActionResult GetGameProgress()
	{
		if (_gameService.GameState is not InProgressState inProgressState)
		{
			return NotFound("Invalid game state");
		}

		return Ok(new GameProgressDto
		{
			AlivePlayers = inProgressState.AlivePlayers,
			TotalPlayers = inProgressState.TotalPlayers
		});
	}

	[HttpGet("winner")]
	public IActionResult GetWinner()
	{
		return _gameService.GetWinner().Match<IActionResult>(
			onSuccess: (winner) => Ok(new GameWinnerDto
			{
				WinnerName = winner.FullName
			}),
			onFailure: (error) => error switch
			{
				GetWinnerErrors.InvalidGameStateError => NotFound("Invalid game state"),
				_ => Problem("An unknown error occurred", statusCode: StatusCodes.Status500InternalServerError)
			});
	}

	[HttpGet("self")]
	public async Task<IActionResult> GetInfoSelf()
	{
		var playerResult = await GetLoggedPlayer();

		return await playerResult.MatchAsync(
			onSuccess: async (player) =>
			{
				var targetResult = await _gameService.GetTarget(player);

				return targetResult.Match<IActionResult>(
					onSuccess: target => Ok(new PlayerInfoDto
					{
						Alive = player.Alive,
						KillCode = player.KillCode,
						TargetName = target.User.FullName
					}),
					onFailure: error => error switch
					{
						GetTargetErrors.TargetUserNotFound => Ok(new PlayerInfoDto
						{
							Alive = player.Alive,
							KillCode = player.KillCode,
							TargetName = string.Empty
						}),
						_ => Problem("An unknown error occurred", statusCode: StatusCodes.Status500InternalServerError)
					}
				);
			},
			onFailure: (error) => error switch
			{
				GetPlayerErrors.UserNotLoggedIn => Unauthorized(),
				GetPlayerErrors.UserDoesNotTakePartInGameError => NotFound("Couldn't find player"),
				GetPlayerErrors.InvalidGameStateError => Conflict("Invalid game state"),
				_ => Problem("An unknown error occurred", statusCode: StatusCodes.Status500InternalServerError)
			});
	}

	private Task<Result<Player, GetPlayerErrors>> GetLoggedPlayer()
	{
		return HttpContext.GetLoggedUser().MatchAsync(
			onSuccess: (user) => _gameService.GetPlayer(user),
			onFailure: (error) => Result<Player, GetPlayerErrors>.Failure(GetPlayerErrors.UserNotLoggedIn));
	}
}
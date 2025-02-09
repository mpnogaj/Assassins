using Assassins.Web.Dto;
using Assassins.Web.Middlewares;
using Assassins.Web.Models;
using Assassins.Web.Services.GameService;
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
	public async Task<IActionResult> ToggleRegister()
	{
		if (_gameService.GameState is not RegistrationState _)
		{
			return Conflict("Invalid game state");
		}

		var user = HttpContext.GetLoggedUser();

		if (user == null)
		{
			return Unauthorized();
		}

		var registrationSuccessful = await _gameService.ToggleRegisterUser(user);

		if (!registrationSuccessful)
		{
			return Forbid();
		}

		return Ok();
	}

	[HttpGet("register")]
	public IActionResult IsRegistered()
	{
		var user = HttpContext.GetLoggedUser();

		if (user == null)
		{
			return Unauthorized();
		}

		return Ok(new RegistrationStatusDto()
		{
			Registered = user.Registered
		});
	}

	[HttpPost("kill")]
	[EnableRateLimiting("fixed")]
	public async Task<IActionResult> Kill([FromBody] KillRequestDto killRequestDto)
	{
		if (_gameService.GameState is not InProgressState _)
		{
			return Conflict("Invalid game state");
		}

		var player = await GetLoggedPlayer();

		if (player == null)
		{
			return Forbid();
		}

		var killSuccessful = await _gameService.AttemptKill(player, killRequestDto.KillCode);

		if (killSuccessful)
		{
			return Ok();
		}
		else
		{
			return Forbid();
		}
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
		if (_gameService.GameState is not FinishedState finishedState)
		{
			return NotFound("Invalid game state");
		}

		return Ok(new GameWinnerDto
		{
			WinnerName = finishedState.Winner.FullName
		});
	}

	[HttpGet("self")]
	public async Task<IActionResult> GetInfoSelf()
	{
		if (_gameService.GameState is not InProgressState _)
		{
			return Conflict("Invalid game state");
		}

		var player = await GetLoggedPlayer();

		if (player == null)
		{
			return Unauthorized();
		}

		var target = await _gameService.GetTarget(player);

		return Ok(new PlayerInfoDto
		{
			Alive = player.Alive,
			KillCode = player.KillCode,
			TargetName = target?.User.FullName ?? string.Empty
		});
	}

	private async Task<Player?> GetLoggedPlayer()
	{
		var user = HttpContext.GetLoggedUser();

		if (user == null) return null;

		return await _gameService.GetPlayer(user);
	}
}
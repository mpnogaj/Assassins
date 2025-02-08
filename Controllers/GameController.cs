using Assassins.Dto;
using Assassins.Middlewares;
using Assassins.Models;
using Assassins.Services.GameService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assassins.Controllers;

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
		if (HttpContext.Items[UserMiddleware.UserKey] is not User user)
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
	public async Task<IActionResult> Kill([FromBody] KillRequestDto killRequestDto)
	{
		var user = HttpContext.GetLoggedUser();
		if (user == null)
		{
			return Unauthorized();
		}

		var player = await _gameService.GetPlayer(user);
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
	public async Task<IActionResult> GetGameProgress()
	{
		if (_gameService.GameState is not InProgressState inProgressState)
		{
			return NotFound();
		}

		var user = HttpContext.GetLoggedUser();

		if (user == null)
		{
			return Unauthorized();
		}

		var player = await _gameService.GetPlayer(user);

		if (player == null)
		{
			// treat user as spectator
			return Ok(new GameProgressDto
			{
				AlivePlayers = inProgressState.AlivePlayers,
				PlayerAlive = null
			});
		}

		return Ok(new GameProgressDto
		{
			AlivePlayers = inProgressState.AlivePlayers,
			PlayerAlive = player.Alive
		});
	}

	[HttpGet("winner")]
	public IActionResult GetWinner()
	{
		if (_gameService.GameState is not FinishedState finishedState)
		{
			return NotFound();
		}

		return Ok(finishedState.Winner.FullName);
	}
}
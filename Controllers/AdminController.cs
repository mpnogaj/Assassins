using Assassins.Dto;
using Assassins.Services.GameService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assassins.Controllers;

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

	[HttpGet("extendedProgress")]
	public async Task<IActionResult> GetExtendedGameProgress()
	{
		if (_gameService.GameState is not InProgressState inProgressState)
		{
			return Conflict();
		}

		var alivePlayers = inProgressState.AlivePlayers;
		var playersWithVTargets = await _gameService.GetPlayersWithTargets();

		var playerData = playersWithVTargets.Select(player => new ExtendedGameProgressDto.PlayerWithTargetDto
		{
			PlayerId = player.Id,
			PlayerFullName = player.User.FullName,
			VictimId = player.TargetGuid,
			VictimFullName = player.Target.User.FullName
		}).ToList();

		var extendedGameProgressDto = new ExtendedGameProgressDto()
		{
			AliveUsers = alivePlayers,
			PlayerData = playerData
		};

		return Ok(extendedGameProgressDto);
	}
}
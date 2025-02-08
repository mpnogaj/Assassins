namespace Assassins.Dto;

public class GameProgressDto
{
	/// <summary>
	/// When null treat user as spectator
	/// </summary>
	public bool? PlayerAlive { get; set; }
	public int AlivePlayers { get; set; }
}
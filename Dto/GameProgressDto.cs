namespace Assassins.Dto;

public class GameProgressDto
{
	/// <summary>
	/// When null treat user as spectator
	/// </summary>
	public bool? UserAlive { get; set; }
	public int AliveUsers { get; set; }
}
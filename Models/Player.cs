namespace Assassins.Models;

/// <summary>
/// Represents player taking part in game
/// </summary>
public class Player
{
	public Guid Id { get; set; }
	public User User { get; set; } = null!;

	public string KillCode { get; set; } = null!;
	public bool Alive { get; set; } = true;

	public Guid TargetGuid { get; set; }
	public virtual Player Target { get; set; } = null!;
}
namespace Assassins.Web.Dto;

public class PlayerInfoDto
{
	public bool Alive { get; set; }
	public string KillCode { get; set; } = null!;
	public string TargetName { get; set; } = null!;
}
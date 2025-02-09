namespace Assassins.Web.Dto;

public class ExtendedGameProgressDto
{
	public class PlayerWithTargetDto
	{
		public bool Alive { get; set; }
		public Guid PlayerId { get; set; }
		public Guid? VictimId { get; set; }
		public string PlayerFullName { get; set; } = null!;
		public string? VictimFullName { get; set; }
	}

	public int AliveUsers { get; set; }
	public List<PlayerWithTargetDto> PlayerData { get; set; } = null!;
}
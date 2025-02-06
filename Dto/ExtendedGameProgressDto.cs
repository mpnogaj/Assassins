namespace Assassins.Dto;

public class ExtendedGameProgressDto
{
	public class PlayerWithTargetDto
	{
		public Guid PlayerId { get; set; }
		public Guid VictimId { get; set; }
		public string PlayerFullName { get; set; } = null!;
		public string VictimFullName { get; set; } = null!;
	}

	public int AliveUsers { get; set; }
	public List<PlayerWithTargetDto> PlayerData { get; set; } = null!;
}
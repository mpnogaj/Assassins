namespace Assassins.Web.Dto;

public class ParticipantsDto
{
	public class UserInfoDto
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = null!;
	}

	public List<UserInfoDto> Participants { get; set; } = null!;
}
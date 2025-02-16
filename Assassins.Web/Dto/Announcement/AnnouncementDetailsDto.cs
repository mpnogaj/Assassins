namespace Assassins.Web.Dto.Announcement;

public class AnnouncementDetailsDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public DateTime Date { get; set; }
}

public class AnnouncementsDto
{
	public List<AnnouncementDetailsDto> Announcements { get; set; } = null!;
}
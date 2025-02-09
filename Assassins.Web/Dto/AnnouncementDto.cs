namespace Assassins.Web.Dto;

public class AnnouncementDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public DateTime Date { get; set; }
}

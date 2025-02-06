namespace Assassins.Models;

public class Announcement
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public DateTime Date { get; set; }
}


using Assassins.Web.Models;

namespace Assassins.Web.Repositories.AnnouncementRepository;

public interface IAnnouncementRepository
{
	public Task<List<Announcement>> GetAnnouncements();
	public Task<Announcement?> GetAnnouncement(Guid announcementId);
	public Task AddAnnouncement(Announcement announcement);
	public Task DeleteAnnouncement(Guid announcementId);
	public Task UpdateAnnouncement(Announcement announcement);
}
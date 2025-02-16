using Assassins.Web.Dto;
using Assassins.Web.Dto.Announcement;
using Assassins.Web.Models;

namespace Assassins.Web.Services.AnnouncementService;

public interface IAnnouncementService
{
	public Task<List<Announcement>> GetAnnouncements();
	public Task AddAnnouncement(AddAnnouncementDto announcement);
	public Task RemoveAnnouncement(Guid announcementId);
}
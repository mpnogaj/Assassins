using Assassins.Web.Dto;
using Assassins.Web.Dto.Announcement;
using Assassins.Web.Hub;
using Assassins.Web.Models;
using Assassins.Web.Repositories.AnnouncementRepository;
using Microsoft.AspNetCore.SignalR;

namespace Assassins.Web.Services.AnnouncementService;

public class AnnouncementService(
	IAnnouncementRepository announcementRepository,
	IHubContext<AssassinsHub, IAssassinsClient> assassinsHubContext) : IAnnouncementService
{


	public async Task<List<Announcement>> GetAnnouncements()
	{
		return await announcementRepository.GetAnnouncements();
	}

	public async Task AddAnnouncement(AddAnnouncementDto addAnnouncementDto)
	{
		var announcement = new Announcement
		{
			Id = Guid.NewGuid(),
			Title = addAnnouncementDto.Title,
			Content = addAnnouncementDto.Content,
			Date = DateTime.Now
		};
		await announcementRepository.AddAnnouncement(announcement);

		await assassinsHubContext.Clients.All.NotifyAnnouncementsChanged();
	}

	public async Task RemoveAnnouncement(Guid announcementId)
	{
		await announcementRepository.DeleteAnnouncement(announcementId);

		await assassinsHubContext.Clients.All.NotifyAnnouncementsChanged();
	}
}

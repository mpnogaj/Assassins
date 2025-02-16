using Assassins.Web.Dto;
using Assassins.Web.Dto.Announcement;
using Assassins.Web.Services.AnnouncementService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assassins.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnnouncementController(IAnnouncementService announcementService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var announcements = await announcementService.GetAnnouncements();
		return Ok(new AnnouncementsDto
		{
			Announcements = announcements.Select(announcement => new AnnouncementDetailsDto
			{
				Id = announcement.Id,
				Title = announcement.Title,
				Content = announcement.Content,
				Date = announcement.Date
			}).ToList()
		});
	}

	[HttpPost("add")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> AddAnnouncement([FromBody] AddAnnouncementDto addAnnouncementDto)
	{
		await announcementService.AddAnnouncement(addAnnouncementDto);
		return Ok();
	}

	[HttpDelete("delete")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> DeleteAnnouncementTask([FromBody] DeleteAnnouncementDto deleteAnnouncementDto)
	{
		await announcementService.RemoveAnnouncement(deleteAnnouncementDto.Id);
		return Ok();
	}
}


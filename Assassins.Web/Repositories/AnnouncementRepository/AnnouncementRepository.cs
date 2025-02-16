using Assassins.Web.Db;
using Assassins.Web.Models;
using Microsoft.EntityFrameworkCore;
namespace Assassins.Web.Repositories.AnnouncementRepository;

public class AnnouncementRepository(AppDbContext dbContext) : IAnnouncementRepository
{
	public async Task<List<Announcement>> GetAnnouncements()
	{
		return await dbContext.Announcements.ToListAsync();
	}

	public async Task<Announcement?> GetAnnouncement(Guid announcementId)
	{
		return await dbContext.Announcements.FirstOrDefaultAsync(announcement => announcement.Id == announcementId);
	}

	public async Task AddAnnouncement(Announcement announcement)
	{
		await dbContext.Announcements.AddAsync(announcement);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAnnouncement(Guid announcementId)
	{
		var announcementToDelete = await GetAnnouncement(announcementId);
		if (announcementToDelete == null)
		{
			return;
		}

		dbContext.Announcements.Remove(announcementToDelete);
		await dbContext.SaveChangesAsync();
	}

	public async Task UpdateAnnouncement(Announcement announcement)
	{
		var entry = dbContext.Entry(announcement);
		if (entry.State == EntityState.Detached)
		{
			var existingEntity = await GetAnnouncement(announcement.Id);
			if (existingEntity == null)
			{
				return;
			}

			dbContext.Entry(existingEntity).CurrentValues.SetValues(announcement);
		}

		dbContext.Announcements.Update(announcement);
	}
}
using Assassins.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Assassins.Web.Db;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions options) : base(options)
	{

	}

	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Player> Players { get; set; } = null!;
	public DbSet<Announcement> Announcements { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(user => user.Id);
		});

		modelBuilder.Entity<Player>(entity =>
		{
			entity.HasKey(player => player.Id);
			entity.HasOne(player => player.User);
		});

		modelBuilder.Entity<Announcement>(entity =>
		{
			entity.HasKey(announcement => announcement.Id);
		});
	}
}
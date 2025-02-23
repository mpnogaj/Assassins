namespace Assassins.Web.Hub;

public interface IAssassinsClient
{
	public Task NotifyGameStateChanged();
	public Task NotifyKillHappened();
	public Task NotifyAnnouncementsChanged();
}
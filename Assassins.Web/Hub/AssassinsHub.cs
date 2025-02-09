using Microsoft.AspNetCore.SignalR;

namespace Assassins.Web.Hub;

public class AssassinsHub : Hub<IAssassinsClient>
{
}
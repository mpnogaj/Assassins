using Microsoft.AspNetCore.Mvc;

namespace Assassins.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
	[HttpGet]
	public IActionResult Pong()
	{
		return Ok("pong");
	}
}

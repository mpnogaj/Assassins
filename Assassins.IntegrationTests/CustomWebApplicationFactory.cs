using Assassins.IntegrationTests.Mocks;
using Assassins.Web;
using Assassins.Web.Db;
using Assassins.Web.Services.RecaptchaService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;

namespace Assassins.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Testing");

		builder.ConfigureTestServices(services =>
		{
			RemoveService<IRecaptchaService>(services);
			RemoveService<AppDbContext>(services);

			services.AddTransient<IRecaptchaService, RecaptchaServiceMock>();
			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlite("DataSource=:memory:");
			});
		});
	}

	private void RemoveService<TService>(IServiceCollection services)
	{
		var descriptor = services
			.SingleOrDefault(d => d.ServiceType == typeof(TService));
		if (descriptor != null)
		{
			services.Remove(descriptor);
		}
	}
}
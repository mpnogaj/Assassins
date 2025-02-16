using Microsoft.Extensions.FileProviders;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;
using Assassins.Web.Services.GameService;
using Assassins.Web.Services.UserService;
using Assassins.Web.Services.JwtService;
using Assassins.Web.Db;
using Assassins.Web.Middlewares;
using Assassins.Web.Hub;
using Assassins.Web.Models;
using Assassins.Web.Repositories.AnnouncementRepository;
using Assassins.Web.Services.RecaptchaService;
using Assassins.Web.Repositories.PlayerRepository;
using Assassins.Web.Repositories.UserRepository;
using Assassins.Web.Services.AnnouncementService;

namespace Assassins.Web
{
	public class Roles
	{
		public const string User = "user";
		public const string Admin = "admin";
	}

	public partial class Program
	{
		private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient();

			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlite(configuration.GetConnectionString("Default"));
				options.EnableSensitiveDataLogging();
			});

			services.AddAuthentication().AddJwtBearer(options =>
			{
				var jwtSecurityKey = configuration["Jwt:SecurityKey"]!;
				var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey));
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = false,
					ValidateIssuer = false,
					IssuerSigningKey = signingKey
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						context.Token = context.Request.Cookies["jwt"];
						return Task.CompletedTask;
					}
				};
			});

			services.AddAuthorization();

			services.AddRateLimiter(options =>
			{
				options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

				options.AddPolicy("fixed", httpContext =>
					RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: httpContext.GetLoggedUser().Match(
							onSuccess: (user) => user.Id.ToString(),
							onFailure: (_) => string.Empty),
						factory: _ => new FixedWindowRateLimiterOptions()
						{
							PermitLimit = 1,
							Window = TimeSpan.FromSeconds(10)
						}));
			});

			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IPlayerRepository, PlayerRepository>();
			services.AddTransient<IAnnouncementRepository, AnnouncementRepository>();

			services.AddTransient<IJwtService, JwtService>();
			services.AddTransient<IRecaptchaService, RecaptchaService>();
			services.AddTransient<IUserService, UserService>();

			services.AddTransient<IGameService, GameService>();

			services.AddTransient<IAnnouncementService, AnnouncementService>();

			services.AddControllers();

			services.AddSignalR();
		}

		private static void EnsureDb(WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			dbContext.Database.EnsureCreated();

			var adminUsers = app.Configuration.GetAdminUsers();

			foreach (var adminUser in adminUsers
						 .Where(adminUser =>
							 dbContext.Users.FirstOrDefault(user => user.Username == adminUser.Username) == null))
			{
				var user = new User(adminUser, BC.HashPassword(adminUser.Password), true);
				dbContext.Users.Add(user);
			}

			var normalUsers = app.Configuration.GetNormalUsers();
			foreach (var normalUser in normalUsers
						 .Where(normalUser =>
							 dbContext.Users.FirstOrDefault(user => user.Username == normalUser.Username) == null))
			{
				var user = new User(normalUser, BC.HashPassword(normalUser.Password));
				dbContext.Users.Add(user);
			}

			dbContext.SaveChanges();
		}

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			ConfigureServices(builder.Services, builder.Configuration);

			var app = builder.Build();

			EnsureDb(app);

			using (var scope = app.Services.CreateScope())
			{
				Console.WriteLine("Opening registration");
				scope.ServiceProvider.GetRequiredService<IGameService>().OpenRegistration();
			}

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseUser();

			app.UseRateLimiter();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");


			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider =
					new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "dist")),
				RequestPath = ""
			});

			app.MapFallbackToFile("/index.html", new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "dist"))
			});

			app.MapHub<AssassinsHub>("/assassins-ws");

			app.Run();
		}
	}
}

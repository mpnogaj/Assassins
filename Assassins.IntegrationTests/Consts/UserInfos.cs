using Assassins.IntegrationTests.Utils.User;

namespace Assassins.IntegrationTests.Consts;

public static class UserInfos
{
	public static readonly UserInfo AdminUser = new UserInfo
	{
		FirstName = "Admin",
		LastName = "Admin",
		Password = "root",
		Username = "root"
	};

	public static readonly UserInfo User1 = new UserInfo
	{
		FirstName = "TestUser",
		LastName = "No1",
		Password = "SuperSecurePswd",
		Username = "user1"
	};

	public static readonly UserInfo User2 = new UserInfo
	{
		FirstName = "TestUser",
		LastName = "No2",
		Password = "SuperSecurePswd",
		Username = "user2"
	};

	public static readonly UserInfo User3 = new UserInfo
	{
		FirstName = "TestUser",
		LastName = "No3",
		Password = "SuperSecurePswd",
		Username = "user3"
	};
}
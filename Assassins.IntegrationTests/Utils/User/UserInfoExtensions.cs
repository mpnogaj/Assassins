using Assassins.Web.Dto;

namespace Assassins.IntegrationTests.Utils.User;

public static class UserInfoExtensions
{
	public static UserRegisterDto ToUserRegisterDto(this UserInfo userInfo)
	{
		return new UserRegisterDto
		{
			FirstName = userInfo.FirstName,
			LastName = userInfo.LastName,
			Username = userInfo.Username,
			Password = userInfo.Password,
			RecaptchaToken = string.Empty
		};
	}

	public static LoginDto ToLoginDto(this UserInfo userInfo)
	{
		return new LoginDto
		{
			Username = userInfo.Username,
			Password = userInfo.Password
		};
	}
}
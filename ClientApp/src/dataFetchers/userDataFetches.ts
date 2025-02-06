import Endpoints from '@/endpoints';
import { UserDto } from '@/types/dto/userDto';
import { sendGet } from '@/utils/fetchUtils';

export const fetchUserInfo = async () => {
	const userDto = await sendGet<UserDto>(Endpoints.user.userInfo);
	return userDto;
};

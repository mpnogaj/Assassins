type UserInfoDto = {
	id: string;
	fullName: string;
};

type ParticipantsDto = {
	participants: UserInfoDto[];
};

export default ParticipantsDto;

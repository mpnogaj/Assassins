type AnnouncementDetailsDto = {
	id: string;
	title: string;
	content: string;
	date: string;
};

type AnnouncementsDto = {
	announcements: AnnouncementDetailsDto[];
};

export default AnnouncementsDto;

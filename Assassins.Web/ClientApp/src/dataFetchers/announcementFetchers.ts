import Endpoints from '@/endpoints';
import AnnouncementsDto from '@/types/dto/announcement/announcementDto';
import { sendGet } from '@/utils/fetchUtils';

export const fetchAnnouncements = () =>
	sendGet<AnnouncementsDto>(Endpoints.announcement.getAnnouncements);

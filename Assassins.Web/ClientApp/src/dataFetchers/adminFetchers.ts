import Endpoints from '@/endpoints';
import ExtendedGameProgressDto from '@/types/dto/admin/extendedGameProgressDto';
import ParticipantsDto from '@/types/dto/admin/participantsDto';
import { sendGet } from '@/utils/fetchUtils';

export const fetchExtendedGameProgress = () =>
	sendGet<ExtendedGameProgressDto>(Endpoints.admin.extendedProgress);

export const fetchParticipants = () => sendGet<ParticipantsDto>(Endpoints.admin.participants);

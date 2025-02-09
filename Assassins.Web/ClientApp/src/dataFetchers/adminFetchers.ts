import Endpoints from '@/endpoints';
import ExtendedGameProgressDto from '@/types/dto/admin/extendedGameProgressDto';
import { sendGet } from '@/utils/fetchUtils';

export const fetchExtendedGameProgress = () =>
	sendGet<ExtendedGameProgressDto>(Endpoints.admin.extendedProgress);

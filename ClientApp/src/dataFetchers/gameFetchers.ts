import Endpoints from '@/endpoints';
import GameProgressDto from '@/types/dto/game/gameProgressDto';
import GameStateDto from '@/types/dto/game/gameStateDto';
import RegistrationStatusDto from '@/types/dto/game/registrationStatusDto';
import { sendGet } from '@/utils/fetchUtils';

export const fetchGameState = () => sendGet<GameStateDto>(Endpoints.game.gameState);

export const fetchRegistrationStatus = () =>
	sendGet<RegistrationStatusDto>(Endpoints.game.register);

export const fetchGameProgress = () => sendGet<GameProgressDto>(Endpoints.game.progress);

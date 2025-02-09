import Endpoints from '@/endpoints';
import GameProgressDto from '@/types/dto/game/gameProgressDto';
import GameStateDto from '@/types/dto/game/gameStateDto';
import GameWinnerDto from '@/types/dto/game/gameWinnerDto';
import PlayerInfoDto from '@/types/dto/game/playerInfoDto';
import RegistrationStatusDto from '@/types/dto/game/registrationStatusDto';
import { sendGet } from '@/utils/fetchUtils';

export const fetchGameState = () => sendGet<GameStateDto>(Endpoints.game.gameState);

export const fetchRegistrationStatus = () =>
	sendGet<RegistrationStatusDto>(Endpoints.game.register);

export const fetchGameProgress = () => sendGet<GameProgressDto>(Endpoints.game.progress);

export const fetchGameWinner = () => sendGet<GameWinnerDto>(Endpoints.game.winner);

export const fetchPlayerData = () => sendGet<PlayerInfoDto>(Endpoints.game.playerInfo);

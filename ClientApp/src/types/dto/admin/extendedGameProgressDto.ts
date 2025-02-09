type PlayerWithTargetDto = {
	alive: boolean;
	playerId: string;
	victimId: string | null;
	playerFullName: string;
	victimFullName: string | null;
};

type ExtendedGameProgressDto = {
	alivePlayers: number;
	playerData: PlayerWithTargetDto[];
};

export default ExtendedGameProgressDto;

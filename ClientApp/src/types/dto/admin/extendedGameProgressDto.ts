type PlayerWithTargetDto = {
	playerId: string;
	victimId: string;
	playerFullName: string;
	victimFullName: string;
};

type ExtendedGameProgressDto = {
	alivePlayers: number;
	playerData: PlayerWithTargetDto[];
};

export default ExtendedGameProgressDto;

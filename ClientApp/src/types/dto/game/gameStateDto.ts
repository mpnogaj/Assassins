type GameStateDto = {
	gameState: string;
};

export const GameStates = {
	UnknownState: 'unknown',
	RegistrationState: 'registration',
	AboutToStartState: 'aboutToStart',
	InProgressState: 'inProgress',
	FinishedState: 'finished'
};

export default GameStateDto;

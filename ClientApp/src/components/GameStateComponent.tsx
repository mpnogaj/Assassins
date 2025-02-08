import GameStateDto, { GameStates } from '@/types/dto/game/gameStateDto';
import { JSX, useEffect } from 'react';
import { fetchGameState } from '@/dataFetchers/gameFetchers';
import { useDataFetch } from '@/hooks/useDataFetch';
import { useWebSocket } from '@/hooks/useWebSocket';
import LoaderComponent from './LoaderComponent';
import FetchErrorComponent from './FetchErrorComponent';

type Props = {
	UnknownStateComponent: JSX.Element;
	RegistrationStateComponent: JSX.Element;
	AboutToStartStateComponent: JSX.Element;
	InProgressStateComponent: JSX.Element;
	FinishedStateComponent: JSX.Element;

	FallbackStateComponent: JSX.Element;
};

const GameStateComponent: React.FC<Props> = props => {
	const { data, isLoading, isError, refetch } = useDataFetch<GameStateDto>(fetchGameState);
	const connection = useWebSocket();

	useEffect(() => {
		if (!connection) return;

		connection.on('NotifyGameStateChanged', () => {
			console.log('Game state changed, updating...');
			refetch();
		});

		return () => {
			connection.off('NotifyGameStateChanged');
		};
	}, [connection]);

	if (isLoading) return <LoaderComponent />;
	if (isError || !data) return <FetchErrorComponent />;

	switch (data.gameState) {
		case GameStates.UnknownState:
			return props.UnknownStateComponent;
		case GameStates.RegistrationState:
			return props.RegistrationStateComponent;
		case GameStates.AboutToStartState:
			return props.AboutToStartStateComponent;
		case GameStates.InProgressState:
			return props.InProgressStateComponent;
		case GameStates.FinishedState:
			return props.FinishedStateComponent;
		default:
			return props.FallbackStateComponent;
	}
};

export default GameStateComponent;

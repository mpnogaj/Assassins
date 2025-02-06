import GameStateDto, { GameStates } from '@/types/dto/game/gameStateDto';
import { JSX } from 'react';
import { FetchableComponent, useFetchableComponent } from './hoc/FetchableComponent';
import { HubConnectionBuilder } from '@microsoft/signalr';
import Endpoints from '@/endpoints';

type Props = {
	UnknownStateComponent: JSX.Element;
	RegistrationStateComponent: JSX.Element;
	AboutToStartStateComponent: JSX.Element;
	InProgressStateComponent: JSX.Element;
	FinishedStateComponent: JSX.Element;

	FallbackStateComponent: JSX.Element;
};

class GameStateComponent extends FetchableComponent<GameStateDto, Props> {
	connection = new HubConnectionBuilder().withUrl(Endpoints.ws).build();

	componentDidMount(): void {
		this.connection.on('NotifyGameStateChanged', () => {
			console.log('game state changed, updating...');
			this.props.refetch();
		});

		this.connection
			.start()
			.then(() => console.log('ws connected'))
			.catch(err => console.error(err));
	}

	componentWillUnmount(): void {
		this.connection.off('NotifyGameStateChanged');
		this.connection.stop();
	}

	render() {
		switch (this.props.data.gameState) {
			case GameStates.UnknownState:
				return this.props.UnknownStateComponent;
			case GameStates.RegistrationState:
				return this.props.RegistrationStateComponent;
			case GameStates.AboutToStartState:
				return this.props.AboutToStartStateComponent;
			case GameStates.InProgressState:
				return this.props.InProgressStateComponent;
			case GameStates.FinishedState:
				return this.props.FinishedStateComponent;
			default:
				return this.props.FallbackStateComponent;
		}
	}
}

export default useFetchableComponent<GameStateDto, Props>(GameStateComponent);

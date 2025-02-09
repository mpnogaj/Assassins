import WebSocketContext from '@/contexts/WebSocketContext';
import Endpoints from '@/endpoints';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNode, useEffect, useState } from 'react';

type Props = {
	children: ReactNode;
};

const connection = new HubConnectionBuilder().withUrl(Endpoints.ws).build();

export const WebSocketProvider = ({ children }: Props) => {
	const [isConnected, setIsConnected] = useState(false);

	useEffect(() => {
		connection
			.start()
			.then(() => {
				console.log('WebSocket connected');
				setIsConnected(true);
			})
			.catch(err => console.error(err));

		return () => {
			connection.stop();
		};
	}, []);

	return (
		<WebSocketContext.Provider value={isConnected ? connection : null}>
			{children}
		</WebSocketContext.Provider>
	);
};

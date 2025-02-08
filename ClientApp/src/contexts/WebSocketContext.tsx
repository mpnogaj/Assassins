import { HubConnection } from '@microsoft/signalr';
import { createContext } from 'react';

const WebSocketContext = createContext<HubConnection | null>(null);
export default WebSocketContext;

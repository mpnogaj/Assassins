import WebSocketContext from '@/contexts/WebSocketContext';
import { useContext } from 'react';

export const useWebSocket = () => useContext(WebSocketContext);

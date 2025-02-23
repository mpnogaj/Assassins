import './style.css';

import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './App';
import { Toaster } from 'react-hot-toast';

const root = document.getElementById('root')!;
createRoot(root).render(
	<StrictMode>
		<Toaster position="top-right" />
		<App />
	</StrictMode>
);

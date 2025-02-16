import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import * as path from 'path';

// https://vite.dev/config/
export default defineConfig({
	server: {
		port: 3000
	},
	resolve: {
		alias: {
			// Define the alias for the `src` folder
			'@': path.resolve(__dirname, 'src')
		},
		extensions: ['.tsx', '.ts', '.jsx', '.js'] // Vite automatically handles extensions, so the ellipsis (`"..."`) is not required
	},
	plugins: [tailwindcss(), react()]
});

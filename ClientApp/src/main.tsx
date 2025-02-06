import 'bootstrap/dist/css/bootstrap.min.css';

import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router';
import App from './App';
import LoginPage from './pages/LoginPage';
import ProtectedRoute from './components/ProtectedRoute';
import { isAdmin, isLoggedIn } from './utils/auth';
import RegisterPage from './pages/RegisterPage';
import LoaderComponent from './components/LoaderComponent';
import AdminPage from './pages/AdminPage';

const root = document.getElementById('root')!;
createRoot(root).render(
	<StrictMode>
		<BrowserRouter>
			<Routes>
				<Route
					path="/"
					element={
						<ProtectedRoute
							checkAuthFunc={isLoggedIn}
							loading={<h1>Loading...</h1>}
							child={<App />}
							fallback={<Navigate to="/login" />}
						/>
					}
				/>
				<Route
					path="/admin"
					element={
						<ProtectedRoute
							checkAuthFunc={isAdmin}
							loading={<LoaderComponent />}
							child={<AdminPage />}
							fallback={<Navigate to="/" />}
						/>
					}
				/>
				<Route path="/login" element={<LoginPage />} />
				<Route path="/register" element={<RegisterPage />} />
			</Routes>
		</BrowserRouter>
	</StrictMode>
);

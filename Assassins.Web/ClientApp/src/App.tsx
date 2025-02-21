import { BrowserRouter, Navigate, Route, Routes } from 'react-router';
import ProtectedRoute from './components/ProtectedRoute';
import Home from './pages/HomePage';
import { isAdmin, isLoggedIn } from './utils/auth';
import LoaderComponent from './components/LoaderComponent';
import AdminPage from './pages/AdminPage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import { WebSocketProvider } from './components/WebSocketProvider';

const App = () => {
	return (
		<div className="flex min-h-screen min-w-screen items-center justify-center bg-gray-900">
			<WebSocketProvider>
				<BrowserRouter>
					<Routes>
						<Route
							path="/"
							element={
								<ProtectedRoute
									checkAuthFunc={isLoggedIn}
									loading={<h1>Loading...</h1>}
									child={<Home />}
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
			</WebSocketProvider>
		</div>
	);
};

export default App;

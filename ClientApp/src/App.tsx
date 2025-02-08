import { BrowserRouter, Navigate, Route, Routes } from 'react-router';
import ProtectedRoute from './components/ProtectedRoute';
import Home from './pages/HomePage';
import { isAdmin, isLoggedIn } from './utils/auth';
import LoaderComponent from './components/LoaderComponent';
import AdminPage from './pages/AdminPage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';

const App = () => {
	return (
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
	);
};

export default App;

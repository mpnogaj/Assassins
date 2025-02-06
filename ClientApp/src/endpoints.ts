const Endpoints = {
	ws: '/assassins-ws',
	user: {
		isLoggedIn: '/user/isLoggedIn',
		login: '/user/login',
		register: '/user/register',
		logout: '/user/logout',
		userInfo: '/user/userInfo'
	},
	admin: {
		isAdmin: '/admin/isAdmin',
		closeRegistration: '/admin/closeRegistration',
		startGame: '/admin/startGame',
		restartGame: '/admin/restartGame',
		extendedProgress: '/admin/extendedProgress'
	},
	game: {
		gameState: '/game/state',
		register: '/game/register'
	}
};

const ApiBase = 'api';

export const createEndpoint = (endpoint: string) => {
	return `${ApiBase}${endpoint}`;
};

export default Endpoints;

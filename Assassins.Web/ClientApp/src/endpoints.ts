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
		extendedProgress: '/admin/extendedProgress',
		adminKill: '/admin/kill',
		participants: '/admin/registeredUsers',
		kickUser: '/admin/kickUser'
	},
	game: {
		gameState: '/game/state',
		register: '/game/register',
		progress: '/game/progress',
		winner: '/game/winner',
		playerInfo: '/game/self',
		kill: '/game/kill'
	},
	announcement: {
		getAnnouncements: '/announcement',
		delete: '/announcement/delete',
		add: '/announcement/add'
	}
};

const ApiBase = 'api';

export const createEndpoint = (endpoint: string) => {
	return `${ApiBase}${endpoint}`;
};

export default Endpoints;

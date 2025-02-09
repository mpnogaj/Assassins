import Endpoints, { createEndpoint } from '../endpoints';

const isHttpOk = async (endpoint: string) => {
	try {
		const result = await fetch(endpoint, {
			credentials: 'include'
		});
		return result.status == 200;
	} catch {
		return false;
	}
};

export const isLoggedIn = () => {
	return isHttpOk(createEndpoint(Endpoints.user.isLoggedIn));
};

export const isAdmin = () => {
	return isHttpOk(createEndpoint(Endpoints.admin.isAdmin));
};

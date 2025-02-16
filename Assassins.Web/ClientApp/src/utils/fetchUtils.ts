import { createEndpoint } from '@/endpoints';

export const sendPost = (endpoint: string, json: unknown | undefined = undefined) => {
	return fetch(createEndpoint(endpoint), {
		headers: {
			'Content-Type': 'application/json'
		},
		method: 'POST',
		body: JSON.stringify(json)
	});
};

export const sendGet = async <T>(endpoint: string): Promise<T | undefined> => {
	try {
		const result = await fetch(createEndpoint(endpoint));
		return result.json() as T;
	} catch {
		return undefined;
	}
};

export const sendDelete = (endpoint: string, json: unknown | undefined = undefined) => {
	return fetch(createEndpoint(endpoint), {
		headers: {
			'Content-Type': 'application/json'
		},
		method: 'DELETE',
		body: JSON.stringify(json)
	});
};

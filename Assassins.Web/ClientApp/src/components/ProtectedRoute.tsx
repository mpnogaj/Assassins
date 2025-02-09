import { useEffect, useState } from 'react';

interface IProps {
	checkAuthFunc: () => Promise<boolean>;
	loading: React.ReactNode;
	child: React.ReactNode;
	fallback: React.ReactNode;
}

const ProtectedRoute = ({ checkAuthFunc, loading, child, fallback }: IProps) => {
	const [state, setState] = useState({ loading: true, authorized: false });

	useEffect(() => {
		const authenticate = async () => {
			let isAuth = false;
			try {
				isAuth = await checkAuthFunc();
			} catch {
				isAuth = false;
			} finally {
				setState({ loading: false, authorized: isAuth });
			}
		};

		authenticate();
	}, [checkAuthFunc]);

	if (state.loading) return loading;
	return state.authorized ? child : fallback;
};

export default ProtectedRoute;

import GameStateComponent from '@/components/GameStateComponent';
import UserInProgressComponent from '@/components/gameStateComponents/user/UserInProgressComponent';
import GameWinnerComponent from '@/components/GameWinnerComponent';
import GreeterComponent from '@/components/GreeterComponent';
import EnterGameComponent from '@/components/EnterGameComponent';
import { WebSocketProvider } from '@/components/WebSocketProvider';
import Endpoints from '@/endpoints';
import { sendPost } from '@/utils/fetchUtils';
import { useNavigate } from 'react-router';

function Home() {
	const navigate = useNavigate();
	const logout = async () => {
		await sendPost(Endpoints.user.logout);
		navigate('./login', { replace: true });
		return true;
	};

	return (
		<div className="flex min-h-screen min-w-screen items-center justify-center bg-gray-900">
			<div className="w-full max-w-2xl rounded-lg bg-gray-800 p-5 shadow-lg">
				<GreeterComponent />
				<WebSocketProvider>
					<GameStateComponent
						RegistrationStateComponent={<EnterGameComponent />}
						AboutToStartStateComponent={
							<h1 className="text-center text-3xl">The game is about to start!</h1>
						}
						InProgressStateComponent={<UserInProgressComponent />}
						FinishedStateComponent={
							<div>
								<h1>Finished state</h1>
								<GameWinnerComponent />
							</div>
						}
						UnknownStateComponent={<h1>Unknown state</h1>}
						FallbackStateComponent={<h1>Fallback state</h1>}
					/>
				</WebSocketProvider>
				<div className="flex align-center justify-end">
					<button
						className="px-4 py-2 rounded-lg text-white font-medium transition-all 
                   bg-red-500 hover:bg-red-600 active:bg-red-700 focus:outline-none"
						onClick={logout}
					>
						Logout
					</button>
				</div>
			</div>
		</div>
	);
}

export default Home;

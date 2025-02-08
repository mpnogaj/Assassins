import ButtonWithNavigation from '@/components/ButtonWithNavigation';
import GameStateComponent from '@/components/GameStateComponent';
import UserInProgressComponent from '@/components/gameStateComponents/user/UserInProgressComponent';
import GreeterComponent from '@/components/GreeterComponent';
import RegistrationComponent from '@/components/RegistrationComponent';
import { WebSocketProvider } from '@/components/WebSocketProvider';
import Endpoints from '@/endpoints';
import { sendPost } from '@/utils/fetchUtils';

function Home() {
	const logout = async () => {
		await sendPost(Endpoints.user.logout);
		return true;
	};

	return (
		<div>
			<GreeterComponent />
			<ButtonWithNavigation
				buttonText="Logout"
				isButton={false}
				destination="/login"
				onClick={logout}
			/>
			<WebSocketProvider>
				<GameStateComponent
					RegistrationStateComponent={<RegistrationComponent />}
					AboutToStartStateComponent={<h1>About to start</h1>}
					InProgressStateComponent={<UserInProgressComponent />}
					FinishedStateComponent={<h1>Finished state</h1>}
					UnknownStateComponent={<h1>Unknown state</h1>}
					FallbackStateComponent={<h1>Fallback state</h1>}
				/>
			</WebSocketProvider>
		</div>
	);
}

export default Home;

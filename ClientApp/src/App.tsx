import ButtonWithNavigation from './components/ButtonWithNavigation';
import GreeterComponent from './components/GreeterComponent';
import { fetchUserInfo } from './dataFetchers/userDataFetches';
import Endpoints from './endpoints';
import { sendPost } from './utils/fetchUtils';

import GameStateComponent from './components/GameStateComponent';
import { fetchGameState, fetchRegistrationStatus } from './dataFetchers/gameFetchers';
import RegistrationComponent from './components/RegistrationComponent';

function App() {
	const logout = async () => {
		await sendPost(Endpoints.user.logout);
		return true;
	};

	return (
		<div>
			<GreeterComponent
				dataFetcher={async () => {
					const userDto = await fetchUserInfo();
					return userDto;
				}}
			/>
			<ButtonWithNavigation
				buttonText="Logout"
				isButton={false}
				destination="/login"
				onClick={logout}
			/>
			<GameStateComponent
				dataFetcher={fetchGameState}
				RegistrationStateComponent={<RegistrationComponent dataFetcher={fetchRegistrationStatus} />}
				AboutToStartStateComponent={<h1>About to start</h1>}
				InProgressStateComponent={<h1>In progress</h1>}
				FinishedStateComponent={<h1>Finished state</h1>}
				UnknownStateComponent={<h1>Unknown state</h1>}
				FallbackStateComponent={<h1>Fallback state</h1>}
			/>
		</div>
	);
}

export default App;

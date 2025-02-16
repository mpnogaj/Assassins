import Announcements from '@/components/AnnouncementsComponent';
import ButtonWithNavigation from '@/components/ButtonWithNavigation';
import GameStateComponent from '@/components/GameStateComponent';
import UserInProgressComponent from '@/components/gameStateComponents/user/UserInProgressComponent';
import GameWinnerComponent from '@/components/GameWinnerComponent';
import GreeterComponent from '@/components/GreeterComponent';
import RegistrationComponent from '@/components/RegistrationComponent';
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
			<GameStateComponent
				RegistrationStateComponent={<RegistrationComponent />}
				AboutToStartStateComponent={<h1>About to start</h1>}
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
			<Announcements showDeleteButton={false} />
		</div>
	);
}

export default Home;

import Endpoints from '@/endpoints';
import { sendPost } from '@/utils/fetchUtils';

const startGameClick = async () => {
	const startGame =
		window.confirm('Are you sure you want to start the game?') &&
		window.confirm('This cannot be undone! Make sure you filtered out all troll players!');

	if (!startGame) return;

	await sendPost(Endpoints.admin.startGame);
};

const AdminAboutToStartComponent = () => {
	return (
		<div>
			<span>Game is about to start</span>
			<span>Here will be list of registered players. Admin should be able to remove them</span>
			<a className="btn btn-primary mt-3" onClick={() => startGameClick()}>
				Start game
			</a>
		</div>
	);
};

export default AdminAboutToStartComponent;

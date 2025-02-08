import Endpoints from '@/endpoints';
import { sendPost } from '@/utils/fetchUtils';

const createNewGame = async () => {
	const startGame = window.confirm('Are you sure you want to start new game?');
	if (!startGame) return;

	await sendPost(Endpoints.admin.restartGame);
};

const AdminFinishedComponent = () => {
	return (
		<div>
			<div>
				<span>Game has finished</span>
				<span>And the winner is: [name]!!11!!1oneone!</span>
			</div>
			<div>
				<a className="btn btn-primary mt-3" onClick={() => createNewGame()}>
					Create new game
				</a>
				<span>*Goes back to registartion phase</span>
			</div>
		</div>
	);
};

export default AdminFinishedComponent;

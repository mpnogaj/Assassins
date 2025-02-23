import GameWinnerComponent from '@/components/GameWinnerComponent';
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
			<div className="flex flex-col justify-center items-center gap-2 mt-4">
				<span className="text-red-500 text-center text-lg">Game has finished</span>
				<div className="text-center">
					<GameWinnerComponent />
				</div>
			</div>
			<div className="flex flex-col justify-center items-center gap-2 mt-4">
				<a
					className="w-full text-center rounded-md bg-blue-600 p-2 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
					onClick={() => createNewGame()}
				>
					Create new game
				</a>
			</div>
		</div>
	);
};

export default AdminFinishedComponent;

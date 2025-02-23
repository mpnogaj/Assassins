import GameProgressComponent from './GameProgress';
import PlayerActionsComponent from './PlayerActions';

const UserInProgressComponent = () => {
	return (
		<div className="text-gray-300 text-center w-full bg-gray-700 rounded-lg shadow-md p-5 gap-4 mb-5 mt-5">
			<GameProgressComponent />
			<PlayerActionsComponent />
		</div>
	);
};

export default UserInProgressComponent;

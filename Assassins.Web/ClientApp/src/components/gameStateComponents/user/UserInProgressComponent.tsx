import GameProgressComponent from './GameProgress';
import PlayerActionsComponent from './PlayerActions';

const UserInProgressComponent = () => {
	return (
		<div className="">
			<GameProgressComponent />
			<PlayerActionsComponent />
		</div>
	);
};

export default UserInProgressComponent;

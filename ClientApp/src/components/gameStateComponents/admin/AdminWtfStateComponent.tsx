import Endpoints from '@/endpoints';
import { sendPost } from '@/utils/fetchUtils';

type Props = {
	GameStateComponentStateName: string;
};

const createNewGame = async () => {
	const startGame = window.confirm('Are you sure you want to start new game?');
	if (!startGame) return;

	await sendPost(Endpoints.admin.restartGame);
};

const AdminWtfStateComponent = (props: Props) => {
	return (
		<div>
			<div>
				<span>Welcome to WTF state</span> <br />
				<span>
					You shouldn't be there. Something wrong happened while processing game states
				</span>{' '}
				<br />
				<span>
					How did I get here? GameStateComponent returned: {props.GameStateComponentStateName}
				</span>{' '}
				<br />
				<span>Don't worry you can try restarting the game</span>
			</div>
			<div>
				<a className="btn btn-primary mt-3" onClick={() => createNewGame()}>
					Restart everything
				</a>
				<span>*Goes back to registartion phase</span>
			</div>
		</div>
	);
};

export default AdminWtfStateComponent;

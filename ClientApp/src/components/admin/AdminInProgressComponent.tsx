import { sendPost } from '@/utils/fetchUtils';
import { FetchableComponent, useFetchableComponent } from '../hoc/FetchableComponent';
import ExtendedGameProgressDto from '@/types/dto/admin/extendedGameProgressDto';
import Endpoints from '@/endpoints';
import AdminKillDto from '@/types/dto/admin/adminKillDto';

class AdminInProgressComponent extends FetchableComponent<ExtendedGameProgressDto> {
	adminKillPlayer = async (playerId: string) => {
		const payload: AdminKillDto = {
			playerGuid: playerId
		};
		try {
			// dont need to call refetch here as parent GameStateComponent will get notified and reloaded
			await sendPost(Endpoints.admin.adminKill, payload);
		} catch {
			alert('Something went wrong xd');
		}
	};

	render() {
		return (
			<div>
				<span>Game in progress</span>
				<span>
					Here will be list of players and their targets. Admin should be able to manually kill in
					case of victim insubordination
				</span>
				<div>
					<table>
						<thead>
							<tr>
								<th>Player ID</th>
								<th>Player Name</th>
								<th>Target ID</th>
								<th>Target Name</th>
								<th>Actions</th>
							</tr>
						</thead>
						<tbody>
							{this.props.data.playerData.map(playerData => {
								// Move into separate component, with btn on click exposed?
								return (
									<tr>
										<th>{playerData.playerId}</th>
										<th>{playerData.playerFullName}</th>
										<th>{playerData.victimId}</th>
										<th>{playerData.victimFullName}</th>
										<th>
											<a className="btn" onClick={() => this.adminKillPlayer(playerData.playerId)}>
												Force kill
											</a>
										</th>
									</tr>
								);
							})}
						</tbody>
					</table>
				</div>
			</div>
		);
	}
}

export default useFetchableComponent(AdminInProgressComponent);

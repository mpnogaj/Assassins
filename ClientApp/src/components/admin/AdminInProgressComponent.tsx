import { FetchableComponent, useFetchableComponent } from '../hoc/FetchableComponent';
import ExtendedGameProgressDto from '@/types/dto/admin/extendedGameProgressDto';

// Should contain ws to update those stats in real life
// to refetch data when on kill
// ws can be shared with user one as it will notify on kill
class AdminInProgressComponent extends FetchableComponent<ExtendedGameProgressDto> {
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
											<a className="btn" onClick={() => alert('todo')}>
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

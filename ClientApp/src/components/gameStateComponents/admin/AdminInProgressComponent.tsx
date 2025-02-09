import { sendPost } from '@/utils/fetchUtils';
import ExtendedGameProgressDto from '@/types/dto/admin/extendedGameProgressDto';
import Endpoints from '@/endpoints';
import AdminKillDto from '@/types/dto/admin/adminKillDto';
import LoaderComponent from '@/components/LoaderComponent';
import FetchErrorComponent from '@/components/FetchErrorComponent';
import { useDataFetch } from '@/hooks/useDataFetch';
import { fetchExtendedGameProgress } from '@/dataFetchers/adminFetchers';
import { useWebSocket } from '@/hooks/useWebSocket';
import { useEffect } from 'react';

const adminKillPlayer = async (playerId: string) => {
	const payload: AdminKillDto = {
		playerGuid: playerId
	};
	try {
		await sendPost(Endpoints.admin.adminKill, payload);
	} catch {
		alert('Something went wrong xd. Maybe the target was already killed?');
	}
};

const AdminInProgressComponent = () => {
	const { data, isLoading, isError, refetch } =
		useDataFetch<ExtendedGameProgressDto>(fetchExtendedGameProgress);
	const _ = refetch;

	const connection = useWebSocket();

	useEffect(() => {
		if (!connection) return;
		console.log('setting up ws NotifyKillHappened event');

		connection.on('NotifyKillHappened', () => {
			console.log('Kill happened, refetching data...');
			refetch();
		});

		return () => {
			connection.off('NotifyKillHappened');
			console.log('clearing ws NotifyKillHappened event');
		};
	}, [connection]);

	if (isLoading) return <LoaderComponent />;
	if (isError || !data) return <FetchErrorComponent />;

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
							<th>Status</th>
							<th>Target ID</th>
							<th>Target Name</th>
							<th>Actions</th>
						</tr>
					</thead>
					<tbody>
						{data.playerData.map(playerData => {
							// Move into separate component, with btn on click exposed?
							return (
								<tr>
									<th>{playerData.playerId}</th>
									<th>{playerData.playerFullName}</th>
									<th>{playerData.alive ? 'Alive' : 'KIA'}</th>
									<th>{playerData.victimId ?? '-'}</th>
									<th>{playerData.victimFullName ?? '-'}</th>
									<th>
										{playerData.alive ? (
											<a className="btn" onClick={() => adminKillPlayer(playerData.playerId)}>
												Force kill
											</a>
										) : (
											<span>-</span>
										)}
									</th>
								</tr>
							);
						})}
					</tbody>
				</table>
			</div>
		</div>
	);
};

export default AdminInProgressComponent;

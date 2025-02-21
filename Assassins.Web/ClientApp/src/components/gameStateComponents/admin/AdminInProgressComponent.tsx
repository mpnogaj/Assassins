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
				<table className="w-full border-collapse border border-gray-600 shadow-md rounded-lg overflow-hidden">
					<thead>
						<tr className="bg-gray-600 text-gray-400 uppercase text-sm font-semibold">
							<th className="px-4 py-2 ">Player ID</th>
							<th className="px-4 py-2 ">Player Name</th>
							<th className="px-4 py-2 ">Status</th>
							<th className="px-4 py-2 ">Target ID</th>
							<th className="px-4 py-2 ">Target Name</th>
							<th className="px-4 py-2 ">Actions</th>
						</tr>
					</thead>
					<tbody>
						{data.playerData.map(playerData => {
							// Move into separate component, with btn on click exposed?
							return (
								<tr className="odd:bg-gray-400 even:bg-gray-400 hover:bg-gray-400 transition">
									<th className="px-4 py-2 ">{playerData.playerId}</th>
									<th className="px-4 py-2 ">{playerData.playerFullName}</th>
									<th className="px-4 py-2 ">{playerData.alive ? 'Alive' : 'KIA'}</th>
									<th className="px-4 py-2 ">{playerData.victimId ?? '-'}</th>
									<th className="px-4 py-2 ">{playerData.victimFullName ?? '-'}</th>
									<th className="px-4 py-2 text-center">
										{playerData.alive ? (
											<a
												className="w-full text-center rounded-md bg-red-600 p-2 text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 mb-2"
												onClick={() => adminKillPlayer(playerData.playerId)}
											>
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

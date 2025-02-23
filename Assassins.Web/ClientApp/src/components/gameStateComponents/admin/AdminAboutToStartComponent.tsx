import LoaderComponent from '@/components/LoaderComponent';
import { fetchParticipants } from '@/dataFetchers/adminFetchers';
import Endpoints from '@/endpoints';
import { useDataFetch } from '@/hooks/useDataFetch';
import KickUserDto from '@/types/dto/admin/kickUserDto';
import ParticipantsDto from '@/types/dto/admin/participantsDto';
import { sendPost } from '@/utils/fetchUtils';

const startGameClick = async () => {
	const startGame =
		window.confirm('Are you sure you want to start the game?') &&
		window.confirm('This cannot be undone! Make sure you filtered out all troll players!');

	if (!startGame) return;

	await sendPost(Endpoints.admin.startGame);
};

const ParticipantsListComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch<ParticipantsDto>(fetchParticipants);

	const kickParticipant = async (particpantId: string) => {
		try {
			const payload: KickUserDto = {
				userId: particpantId
			};
			await sendPost(Endpoints.admin.kickUser, payload);
			refetch();
		} catch {
			alert('Error while kicking user');
		}
	};

	if (isLoading) {
		return <LoaderComponent />;
	} else if (isError || !data) {
		return <LoaderComponent />;
	}

	return (
		<table className="w-full border-collapse border border-gray-600 shadow-md rounded-lg overflow-hidden">
			<thead>
				<tr className="bg-gray-600 text-gray-400 uppercase text-sm font-semibold">
					<th className="px-4 py-2 ">Name</th>
					<th className="px-4 py-2 ">Action</th>
				</tr>
			</thead>
			<tbody>
				{data.participants.map(participant => {
					return (
						<tr
							key={participant.id}
							className="odd:bg-gray-400 even:bg-gray-400 hover:bg-gray-400 transition"
						>
							<th className="px-4 py-2 ">{participant.fullName}</th>
							<th className="px-4 py-2 text-center">
								<a
									className="w-full text-center rounded-md bg-red-600 p-2 text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 mb-2"
									onClick={() => kickParticipant(participant.id)}
								>
									Kick
								</a>
							</th>
						</tr>
					);
				})}
			</tbody>
		</table>
	);
};

const AdminAboutToStartComponent = () => {
	return (
		<div className="flex flex-col justify-center items-center gap-4 mt-4">
			<span className="text-green-500 text-center text-lg">Game is about to start</span>
			<span className="text-red-500 text-center text-lg">
				Here will be list of registered players. Admin should be able to remove them
			</span>
			<a
				className="w-full text-center rounded-md bg-green-600 p-2 text-white hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 mb-2"
				onClick={() => startGameClick()}
			>
				Start game
			</a>
			<h2 className="font-semibold text-gray-400">Participants</h2>
			<ParticipantsListComponent />
		</div>
	);
};

export default AdminAboutToStartComponent;

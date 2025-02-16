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
		<table>
			<thead>
				<tr>
					<th>Name</th>
					<th>Action</th>
				</tr>
			</thead>
			<tbody>
				{data.participants.map(participant => {
					return (
						<tr key={participant.id}>
							<th>{participant.fullName}</th>
							<th>
								<a onClick={() => kickParticipant(participant.id)}>Kick</a>
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
		<div>
			<span>Game is about to start</span>
			<span>Here will be list of registered players. Admin should be able to remove them</span>
			<a className="btn btn-primary mt-3" onClick={() => startGameClick()}>
				Start game
			</a>
			<h2>Participants</h2>
			<ParticipantsListComponent />
		</div>
	);
};

export default AdminAboutToStartComponent;

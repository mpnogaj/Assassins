import { fetchAnnouncements } from '@/dataFetchers/announcementFetchers';
import { useDataFetch } from '@/hooks/useDataFetch';
import { useWebSocket } from '@/hooks/useWebSocket';
import AnnouncementsDto from '@/types/dto/announcement/announcementDto';
import { useEffect } from 'react';
import LoaderComponent from './LoaderComponent';
import FetchErrorComponent from './FetchErrorComponent';
import { dateToString, insertBetween } from '@/utils/others';
import { sendDelete } from '@/utils/fetchUtils';
import Endpoints from '@/endpoints';
import DeleteAnnouncementDto from '@/types/dto/announcement/deleteAnnouncementDto';

type Props = {
	showDeleteButton: boolean;
};

const deleteAnnouncement = async (id: string) => {
	try {
		const payload: DeleteAnnouncementDto = {
			id: id
		};
		console.log(payload);
		await sendDelete(Endpoints.announcement.delete, payload);
	} catch {
		alert('Error occured while deleting announcement');
	}
};

const Announcements = ({ showDeleteButton }: Props) => {
	const { data, isLoading, isError, refetch } = useDataFetch<AnnouncementsDto>(fetchAnnouncements);
	const connection = useWebSocket();

	useEffect(() => {
		if (!connection) return;

		connection.on('NotifyAnnouncementsChanged', () => {
			console.log('Announcement added, updating...');
			refetch();
		});

		return () => {
			connection.off('NotifyAnnouncementsChanged');
		};
	}, [connection]);

	if (isLoading) {
		<LoaderComponent />;
	} else if (isError || !data) {
		<FetchErrorComponent />;
	} else {
		console.log(data);
		return (
			<div>
				<h3>Announcements</h3>
				{insertBetween(
					data.announcements.map(announcement => {
						return (
							<div>
								<h5>{announcement.title}</h5>
								<span>{dateToString(new Date(announcement.date))}</span>
								<p style={{ whiteSpace: 'pre-line' }}>{announcement.content}</p>
								{showDeleteButton ? (
									<a onClick={() => deleteAnnouncement(announcement.id)}>Delete</a>
								) : null}
							</div>
						);
					}),
					<hr />
				)}
			</div>
		);
	}
};

export default Announcements;

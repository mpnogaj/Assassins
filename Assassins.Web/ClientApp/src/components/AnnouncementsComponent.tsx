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
			<div className="space-y-4">
				<h3 className="text-xl font-semibold text-gray-400">Announcements</h3>
				{insertBetween(
					data.announcements.map(announcement => {
						return (
							<div className="bg-gray-500 rounded-lg shadow-md p-4">
								<div className="flex justify-between mb-2">
									<h5 className="text-gray-900 font-bold text-lg break-words text-pretty">
										{announcement.title}
									</h5>
									<span className="text-gray-900 font-light text-sm">
										{dateToString(new Date(announcement.date))}
									</span>
								</div>
								<p className="break-words text-pretty whitespace-pre-line mb-2">
									{announcement.content}
								</p>
								{showDeleteButton ? (
									<a
										className="px-4 py-2 rounded-lg text-white font-medium transition-all 
                   bg-red-500 hover:bg-red-600 active:bg-red-700 focus:outline-none"
										onClick={() => deleteAnnouncement(announcement.id)}
									>
										Delete
									</a>
								) : null}
							</div>
						);
					}),
					null
				)}
			</div>
		);
	}
};

export default Announcements;

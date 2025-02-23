import Endpoints from '@/endpoints';
import AddAnnouncementDto from '@/types/dto/announcement/addAnnouncementDto';
import { sendPost } from '@/utils/fetchUtils';
import { useState } from 'react';

const addAnnouncement = async (title: string, content: string) => {
	try {
		const payload: AddAnnouncementDto = {
			title: title,
			content: content
		};

		await sendPost(Endpoints.announcement.add, payload);
	} catch {
		alert('Error while adding announcement');
	}
};

const AddAnnouncementComponent = () => {
	const [title, setTitle] = useState('');
	const [content, setContent] = useState('');

	return (
		<div className="flex flex-col items-center mt-5 mb-5">
			<h4 className="text-center text-lg text-gray-400">Add Announcement</h4>
			<form
				className="space-y-4"
				onSubmit={e => {
					e.preventDefault();
					addAnnouncement(title, content);
				}}
			>
				<div>
					<label className="block text-sm font-medium text-gray-500">Title:</label>
					<input
						className="mt-1 w-full rounded-md bg-gray-600 p-2"
						type="text"
						onChange={e => setTitle(e.target.value)}
						value={title}
					></input>
				</div>
				<div>
					<label className="block text-sm font-medium text-gray-500">Content:</label>
					<textarea
						className="mt-1 w-full rounded-md bg-gray-600 p-2"
						cols={50}
						onChange={e => setContent(e.target.value)}
					>
						{content}
					</textarea>
				</div>
				<button
					className="w-full rounded-md bg-blue-600 p-2 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
					type="submit"
				>
					Add
				</button>
			</form>
		</div>
	);
};

export default AddAnnouncementComponent;

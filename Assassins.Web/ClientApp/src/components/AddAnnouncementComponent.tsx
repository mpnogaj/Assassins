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
		<div>
			<h4>Add Announcement</h4>
			<form
				onSubmit={e => {
					e.preventDefault();
					addAnnouncement(title, content);
				}}
			>
				<div>
					<label>Title:</label>
					<br />
					<input type="text" onChange={e => setTitle(e.target.value)} value={title}></input>
				</div>
				<div>
					<label>Content:</label>
					<br />
					<textarea cols={50} onChange={e => setContent(e.target.value)}>
						{content}
					</textarea>
				</div>
				<button type="submit">Add</button>
			</form>
		</div>
	);
};

export default AddAnnouncementComponent;

import Endpoints from '@/endpoints';
import { sendPost } from '@/utils/fetchUtils';

const closeRegistrationClick = async () => {
	const closeRegistration = window.confirm('Are you sure you want to close registration?');
	if (!closeRegistration) return;

	await sendPost(Endpoints.admin.closeRegistration);
};

const AdminRegistrationComponent = () => {
	return (
		<div>
			<span>Registration in progress</span>
			<a className="btn btn-primary mt-3" onClick={() => closeRegistrationClick()}>
				Close registration
			</a>
		</div>
	);
};

export default AdminRegistrationComponent;

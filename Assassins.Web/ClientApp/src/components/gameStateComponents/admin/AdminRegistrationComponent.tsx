import Endpoints from '@/endpoints';
import { sendPost } from '@/utils/fetchUtils';

const closeRegistrationClick = async () => {
	const closeRegistration = window.confirm('Are you sure you want to close registration?');
	if (!closeRegistration) return;

	await sendPost(Endpoints.admin.closeRegistration);
};

const AdminRegistrationComponent = () => {
	return (
		<div className="flex flex-col justify-center items-center gap-1 mt-4">
			<span className="text-green-500 text-lg font-semibold mb-2">Registration in progress</span>
			<a
				className="w-full text-center rounded-md bg-red-600 p-2 text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500"
				onClick={() => closeRegistrationClick()}
			>
				Close registration
			</a>
		</div>
	);
};

export default AdminRegistrationComponent;

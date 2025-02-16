import RegistrationStatusDto from '@/types/dto/game/registrationStatusDto';
import { sendPost } from '@/utils/fetchUtils';
import Endpoints from '@/endpoints';
import { useDataFetch } from '@/hooks/useDataFetch';
import { fetchRegistrationStatus } from '@/dataFetchers/gameFetchers';
import LoaderComponent from './LoaderComponent';
import FetchErrorComponent from './FetchErrorComponent';

const EnterGameComponent = () => {
	const { data, isLoading, isError, refetch } =
		useDataFetch<RegistrationStatusDto>(fetchRegistrationStatus);

	const registerBtnHandler = async () => {
		await sendPost(Endpoints.game.register);
		refetch();
	};

	if (isLoading) return <LoaderComponent />;
	if (isError || !data) return <FetchErrorComponent />;

	return (
		<div className="text-center p-4 m-0">
			<h2 className="text-xl font-semibold">
				Participation: {data.registered ? 'confirmed' : 'pending'}
			</h2>
			<button
				className="mt-3 px-4 py-2 rounded-lg text-white font-medium transition-all 
                   bg-blue-500 hover:bg-blue-600 active:bg-blue-700 focus:outline-none"
				onClick={() => registerBtnHandler()}
			>
				{!data.registered ? 'Enter Game' : 'Leave Game'}
			</button>
		</div>
	);
};

export default EnterGameComponent;

import RegistrationStatusDto from '@/types/dto/game/registrationStatusDto';
import { sendPost } from '@/utils/fetchUtils';
import Endpoints from '@/endpoints';
import { useDataFetch } from '@/hooks/useDataFetch';
import { fetchRegistrationStatus } from '@/dataFetchers/gameFetchers';
import LoaderComponent from './LoaderComponent';
import FetchErrorComponent from './FetchErrorComponent';

const RegistrationComponent = () => {
	const { data, isLoading, isError, refetch } =
		useDataFetch<RegistrationStatusDto>(fetchRegistrationStatus);

	const registerBtnHandler = async () => {
		await sendPost(Endpoints.game.register);
		refetch();
	};

	if (isLoading) return <LoaderComponent />;
	if (isError || !data) return <FetchErrorComponent />;

	return (
		<div>
			<h2>You {data.registered ? 'are' : 'are not'} registered</h2>
			<a className="btn btn-primary mt-3" onClick={() => registerBtnHandler()}>
				{!data.registered ? 'Register' : 'Unregister'}
			</a>
		</div>
	);
};

export default RegistrationComponent;

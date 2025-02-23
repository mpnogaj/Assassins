import { useDataFetch } from '@/hooks/useDataFetch';
import { UserDto } from '@/types/dto/userDto';
import LoaderComponent from './LoaderComponent';
import FetchErrorComponent from './FetchErrorComponent';
import { fetchUserInfo } from '@/dataFetchers/userDataFetches';

const GreeterComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch<UserDto>(fetchUserInfo);
	const _ = refetch;

	if (isLoading) return <LoaderComponent />;
	if (isError || !data) return <FetchErrorComponent />;

	return (
		<div className="text-center text-2xl text-gray-400 mb-2">
			<h1 className="font-bold">Hej {data.fullName} - Walcz synu!</h1>
		</div>
	);
};

export default GreeterComponent;

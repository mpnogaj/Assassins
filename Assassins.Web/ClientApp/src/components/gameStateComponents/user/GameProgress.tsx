import FetchErrorComponent from '@/components/FetchErrorComponent';
import LoaderComponent from '@/components/LoaderComponent';
import { fetchGameProgress } from '@/dataFetchers/gameFetchers';
import { useDataFetch } from '@/hooks/useDataFetch';

const GameProgressComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch(fetchGameProgress);
	const _ = refetch;

	if (isLoading) {
		return <LoaderComponent />;
	}

	if (isError || data == undefined) {
		return <FetchErrorComponent />;
	}

	return (
		<div className="text-gray-300 text-center gap-4 mb-5">
			<h4 className="text-lg font-semibold">
				Pozostało graczy: {data.alivePlayers}/{data.totalPlayers}
			</h4>
		</div>
	);
};

export default GameProgressComponent;

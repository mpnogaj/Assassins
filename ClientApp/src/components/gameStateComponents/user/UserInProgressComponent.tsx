import FetchErrorComponent from '@/components/FetchErrorComponent';
import LoaderComponent from '@/components/LoaderComponent';
import { fetchGameProgress } from '@/dataFetchers/gameFetchers';
import { useDataFetch } from '@/hooks/useDataFetch';

const PlayerAliveComponent = (props: { playerAlive: boolean | null }) => {
	const { playerAlive } = props;

	if (playerAlive == null) {
		return <h2>Miłego dnia podglądaczu</h2>;
	} else if (playerAlive) {
		return <h2>Dalej żyjesz - Walcz synu!</h2>;
	} else {
		return <h2>Jesteś martwy - Przegrałeś synu...</h2>;
	}
};

const GameProgressComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch(fetchGameProgress);
	const _1 = refetch;

	if (isLoading) {
		return <LoaderComponent />;
	}

	if (isError || data == undefined) {
		return <FetchErrorComponent />;
	}

	return (
		<div>
			<h2>
				<PlayerAliveComponent playerAlive={data.playerAlive} />
			</h2>
			<h4>Żywi gracze: {data.alivePlayers}</h4>
		</div>
	);
};

const UserInProgressComponent = () => {
	return (
		<div>
			<GameProgressComponent />
		</div>
	);
};

export default UserInProgressComponent;

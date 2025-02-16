import LoaderComponent from '@/components/LoaderComponent';
import { useDataFetch } from '@/hooks/useDataFetch';
import { useSearchParams } from 'react-router';
import PlayerInfoComponent from './PlayerInfo';
import { fetchPlayerData } from '@/dataFetchers/gameFetchers';
import KillComponent from './KillComponent';

const PlayerActionsComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch(fetchPlayerData);
	const [searchParams, setSearchParams] = useSearchParams();

	const _1 = refetch;
	const _2 = setSearchParams;

	if (isLoading) {
		return <LoaderComponent />;
	}

	if (isError || data == undefined) {
		return null;
	}

	const maybeSearchParams = searchParams.get('killCode');

	return (
		<div className="flex flex-col items-center w-full gap-4 mb-4">
			<PlayerInfoComponent alive={data.alive} killCode={data.killCode} />
			<KillComponent
				targetName={data.targetName}
				initialTextBoxContent={(maybeSearchParams as string) ?? ''}
			/>
		</div>
	);
};

export default PlayerActionsComponent;

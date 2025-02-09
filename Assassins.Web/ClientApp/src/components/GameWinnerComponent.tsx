import { fetchGameWinner } from '@/dataFetchers/gameFetchers';
import { useDataFetch } from '@/hooks/useDataFetch';
import GameWinnerDto from '@/types/dto/game/gameWinnerDto';
import LoaderComponent from './LoaderComponent';

const GameWinnerComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch<GameWinnerDto>(fetchGameWinner);
	const _ = refetch;

	if (isLoading) return <LoaderComponent />;
	if (isError || !data) return <p>Couldn't load winner</p>;

	return <span>And the winner is: {data?.winnerName}!!11!!1oneone!</span>;
};

export default GameWinnerComponent;
